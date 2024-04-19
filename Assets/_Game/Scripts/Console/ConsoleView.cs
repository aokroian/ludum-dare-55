using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Common;
using _Game.Scripts.GameLoop;
using _Game.Scripts.Summon.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using static _Game.Scripts.Common.ConsoleStrings;

namespace _Game.Scripts.Console
{
    public class ConsoleView : MonoBehaviour
    {
        [SerializeField] private bool skipInitAnimation;
        [SerializeField] private TextMeshProUGUI inputFieldSenderPart;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private TMP_InputField output;
        [SerializeField] private int maxConsoleOutputLines = 150;
        [SerializeField] private ScrollRect consoleScrollRect;
        [SerializeField] private ContentSizeFitter[] contentSizeFitters;
        [SerializeField] private PointerDownHandler[] pointerDownHandlers;

        [Inject] private ConsoleCommandsHandler _commandsHandler;
        [Inject] private SoundManager _soundManager;
        [Inject] private GlobalInputSwitcher _globalInputSwitcher;
        [Inject] private SummonedObjectsHolder _summonedObjectsHolder;
        [Inject] private GameLoopController _gameLoopController;

        private float _lastSummonGameCommandTime;
        private float _lastHelpTime;

        private readonly List<string> _commandsHistory = new();
        private bool IsInHistoryMode => _currentHistoryIndex != -1;
        private int _currentHistoryIndex = -1;
        private string _inputBeforeUsingHistory;
        private string _animationInputText;
        private int _outputsCount;

        private string _currentOutputText = "";
        private int _currentOutputCaretPosition;

        private bool IsListeningToConsoleInputShortcuts => !_isFocusedOnOutput && _isGlobalConsoleInput;
        private bool _isGlobalConsoleInput = true;
        private bool _isFocusedOnOutput;

        private void Awake()
        {
            foreach (var pointerDownHandler in pointerDownHandlers)
                pointerDownHandler.OnPointerDownEvent += OnPointerDown;
            _lastHelpTime = Time.time - 1000;
        }

        private void OnDestroy()
        {
            foreach (var pointerDownHandler in pointerDownHandlers)
                pointerDownHandler.OnPointerDownEvent -= OnPointerDown;
        }

        public void OnPointerDown()
        {
            _globalInputSwitcher.SwitchToConsoleControls();
        }

        public void OnGlobalInputSwitch(bool isConsoleInputOn)
        {
            _isFocusedOnOutput = false;
            _isGlobalConsoleInput = isConsoleInputOn;
            inputField.interactable = isConsoleInputOn;
            foreach (var pointerDownHandler in pointerDownHandlers)
                pointerDownHandler.gameObject.SetActive(!isConsoleInputOn);

            if (isConsoleInputOn)
                SetInputText(inputField.text);
            else
                consoleScrollRect.verticalNormalizedPosition = 0;
        }

        public void Init()
        {
            _isFocusedOnOutput = false;
            _commandsHistory.Clear();
            inputField.onSubmit.RemoveAllListeners();
            inputField.onSubmit.AddListener(OnCommandSubmit);
            inputField.onEndEdit.RemoveAllListeners();
            inputField.onEndEdit.AddListener(OnEditEnd);
            inputField.onValueChanged.RemoveAllListeners();
            inputField.onValueChanged.AddListener(OnInputValueChanged);

            inputField.onSelect.RemoveAllListeners();
            inputField.onSelect.AddListener(_ =>
            {
                _isFocusedOnOutput = false;
            });

            output.onSelect.RemoveAllListeners();
            output.onSelect.AddListener(_ =>
            {
                _currentOutputCaretPosition = output.caretPosition;
                _isFocusedOnOutput = true;
            });

            output.onValueChanged.RemoveAllListeners();
            output.onValueChanged.AddListener(_ =>
            {
                output.SetTextWithoutNotify(_currentOutputText);
                output.caretPosition = _currentOutputCaretPosition;
                _isFocusedOnOutput = true;
            });

            consoleScrollRect.verticalNormalizedPosition = 0;
            output.SetTextWithoutNotify(_currentOutputText);

            if (skipInitAnimation)
            {
                inputField.SetTextWithoutNotify(GameRestartCommand);
                inputFieldSenderPart.text = YouMainInputSenderText;
                OnCommandSubmit(inputField.text);
            }
            else
            {
                StartCoroutine(GameInitAnimation());
            }
        }

        private void OnInputValueChanged(string currentInputValue)
        {
            _isFocusedOnOutput = false;
            consoleScrollRect.verticalNormalizedPosition = 0;
            _currentHistoryIndex = -1;
        }

        private void OnCommandSubmit(string text)
        {
            if (_animationInputText != null)
                return;
            _currentHistoryIndex = -1;
            _soundManager.PlaySubmitKeySound();

            text = text.Trim();
            // delete \u200E symbol 
            text = text.Replace("\u200E", "");

            _commandsHandler.SubmitCommand(text.ToLower(), OnCommandProcessed);

            inputField.caretPosition = inputField.text.Length;
            inputField.ActivateInputField();
            inputField.Select();
        }

        private void OnCommandProcessed(ConsoleOutputData data, string originalInputText)
        {
            if (originalInputText == GameRestartCommand)
                _lastSummonGameCommandTime = Time.time;

            DisplayNewOutputEntry(data);
            if (originalInputText != "")
                _commandsHistory.Add(originalInputText);
            inputField.text = "";
        }

        public void DisplayNewOutputEntry(ConsoleOutputData data, bool isFromSpeaker = false)
        {
            var addedText = $"\n{data.senderText}{data.messageText}{OutputEndInvisibleSymbol}";
            _currentOutputText += addedText;
            output.SetTextWithoutNotify(_currentOutputText);
            consoleScrollRect.verticalNormalizedPosition = 0;
            _outputsCount++;
            Invoke(nameof(ToggleUI), .05f);

            if (_outputsCount > maxConsoleOutputLines)
            {
                // remove text from the beginning of output.text to the first ConsoleOutputEndInvisibleSymbol symbol
                var firstInvisibleSymbolIndex =
                    _currentOutputText.IndexOf(OutputEndInvisibleSymbol, StringComparison.Ordinal) +
                    OutputEndInvisibleSymbol.Length;
                output.SetTextWithoutNotify(_currentOutputText[firstInvisibleSymbolIndex..]);
                _outputsCount--;
            }
        }

        private void ToggleUI()
        {
            foreach (var c in contentSizeFitters)
            {
                c.enabled = false;
                c.enabled = true;
            }

            Canvas.ForceUpdateCanvases();
        }

        public void EnableInput()
        {
            _animationInputText = null;
        }

        public void DisableInput()
        {
            _animationInputText = "";
        }

        private void OnEditEnd(string inputText)
        {
            // to lower case
            inputField.text = inputText.ToLower();
            _currentHistoryIndex = -1;
        }

        private void Update()
        {
            if (_animationInputText != null)
            {
                SetInputText(_animationInputText);
                return;
            }

            if (!IsListeningToConsoleInputShortcuts) // if player controls are on
                return;

            CheckIfPlayerDoesntGetItAndHelp();

            if (Input.GetKeyDown(KeyCode.UpArrow))
                GoThroughHistory(-1);
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                GoThroughHistory(1);

            if (IsInHistoryMode)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    _currentHistoryIndex = -1;
                    return;
                }

                SetInputText(_commandsHistory[_currentHistoryIndex]);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                AutoComplete();
            }

            // clear output
            if ((Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand)) &&
                Input.GetKeyDown(KeyCode.K))
            {
                _currentOutputText = "";
                output.SetTextWithoutNotify(_currentOutputText);
                _outputsCount = 0;
                SetInputText(inputField.text);
            }
        }

        private void GoThroughHistory(int direction)
        {
            if (direction == -1)
            {
                if (!IsInHistoryMode)
                {
                    _inputBeforeUsingHistory = inputField.text;
                    _currentHistoryIndex = _commandsHistory.Count - 1;
                }
                else if (_currentHistoryIndex > 0)
                    _currentHistoryIndex--;
            }
            else
            {
                if (IsInHistoryMode)
                    _currentHistoryIndex++;
                if (_currentHistoryIndex >= _commandsHistory.Count)
                    _currentHistoryIndex = -1;
                if (!IsInHistoryMode)
                    SetInputText(_inputBeforeUsingHistory);
            }
        }

        private void SetInputText(string text)
        {
            inputField.SetTextWithoutNotify(text);
            inputField.caretPosition = inputField.text.Length;
            inputField.ActivateInputField();
            inputField.Select();
        }

        private void AutoComplete()
        {
            var inputArray = inputField.text.Split(" ");
            if (inputArray.Length == 1)
            {
                if ("summon".StartsWith(inputField.text.ToLower()))
                {
                    inputField.text = "summon ";
                    inputField.caretPosition = inputField.text.Length;
                }
            }
            else if (inputArray.Length == 2 && !string.IsNullOrWhiteSpace(inputArray[1]))
            {
                var knownCommands = _commandsHandler.PermanentGameState.knownCommands;
                foreach (var command in knownCommands)
                {
                    var fullCommand = $"summon {command}";
                    if (fullCommand.StartsWith(inputField.text.ToLower()))
                    {
                        inputField.text = fullCommand;
                        inputField.caretPosition = inputField.text.Length;
                        break;
                    }
                }
            }
        }

        private IEnumerator GameInitAnimation()
        {
            inputFieldSenderPart.text = DevsMainInputSenderText;
            var delays = new[] { .25f, .25f, .1f, .35f, .3f, .7f, .15f, .25f, .28f, .2f, .2f };
            inputField.ActivateInputField();
            inputField.Select();
            _animationInputText = "";
            yield return new WaitForSeconds(2.5f);
            for (var i = 0; i < GameRestartCommand.Length; i++)
            {
                _soundManager.PlayTypeSound();
                _animationInputText = GameRestartCommand.Take(i + 1).Aggregate("", (current, c) => current + c);
                yield return new WaitForSeconds(delays[i]);
            }

            yield return new WaitForSeconds(2.5f);
            _animationInputText = null;
            OnCommandSubmit(GameRestartCommand);
            inputFieldSenderPart.text = YouMainInputSenderText;
        }


        private void CheckIfPlayerDoesntGetItAndHelp()
        {
            if (Mathf.Abs(Time.time - _lastHelpTime) > 20)
            {
                if (!_gameLoopController.IsGameEnded && Time.time - _lastSummonGameCommandTime > 10 &&
                    _summonedObjectsHolder?.RealRoomCount == 0)
                {
                    HelpWithRoomCommand();
                }
            }
        }

        private void HelpWithRoomCommand()
        {
            var outputData = new ConsoleOutputData
            {
                senderText = GetSpeakerString(DevsSpeakerName),
                messageText = GetHintAboutSummoningRoomMessage()
            };
            DisplayNewOutputEntry(outputData);
            _lastHelpTime = Time.time;
        }
    }
}