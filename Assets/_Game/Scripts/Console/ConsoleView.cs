using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Common;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace _Game.Scripts.Console
{
    public class ConsoleView : MonoBehaviour
    {
        [SerializeField] private bool skipInitAnimation;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private ScrollRect outputScrollRect;
        [SerializeField] private Transform outputContainer;
        [SerializeField] private ContentSizeFitter[] contentSizeFitters;
        [SerializeField] private ConsoleOutputEntry outputEntryPrefab;
        [SerializeField] private int maxOutputEntries = 100;
        [SerializeField] private PointerDownHandler[] pointerDownHandlers;

        [Inject] private ConsoleCommandsHandler _commandsHandler;
        [Inject] private SoundManager _soundManager;
        [Inject] private GlobalInputSwitcher _globalInputSwitcher;
        private readonly List<string> _commandsHistory = new();
        private bool IsInHistoryMode => _currentHistoryIndex != -1;
        private int _currentHistoryIndex = -1;
        private string _inputBeforeUsingHistory;
        private string _animationInputText;

        private void Awake()
        {
            foreach (var pointerDownHandler in pointerDownHandlers)
                pointerDownHandler.OnPointerDownEvent += OnPointerDown;
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
            if (isConsoleInputOn)
            {
                inputField.interactable = true;
                SetInputText(inputField.text);
                foreach (var pointerDownHandler in pointerDownHandlers)
                    pointerDownHandler.gameObject.SetActive(false);
            }
            else
            {
                inputField.interactable = false;
                foreach (var pointerDownHandler in pointerDownHandlers)
                    pointerDownHandler.gameObject.SetActive(true);
            }
        }

        public void Init()
        {
            _commandsHistory.Clear();
            inputField.onSubmit.RemoveAllListeners();
            inputField.onSubmit.AddListener(OnCommandSubmit);
            inputField.onEndEdit.RemoveAllListeners();
            inputField.onEndEdit.AddListener(OnEditEnd);
            inputField.onValueChanged.RemoveAllListeners();
            inputField.onValueChanged.AddListener(OnInputValueChanged);

            outputScrollRect.verticalNormalizedPosition = 0;
            foreach (Transform c in outputContainer)
            {
                Destroy(c.gameObject);
            }

            if (skipInitAnimation)
            {
                inputField.SetTextWithoutNotify("summon game");
                OnCommandSubmit(inputField.text);
            }
            else
            {
                StartCoroutine(GameInitAnimation());
            }
        }

        private void OnInputValueChanged(string currentInputValue)
        {
            outputScrollRect.verticalNormalizedPosition = 0;
            _currentHistoryIndex = -1;
        }

        private void OnCommandSubmit(string text)
        {
            if (_animationInputText != null)
                return;
            _currentHistoryIndex = -1;
            _soundManager.PlaySubmitKeySound();
            _commandsHandler.SubmitCommand(text.ToLower(), OnCommandProcessed);

            inputField.caretPosition = inputField.text.Length;
            inputField.ActivateInputField();
            inputField.Select();
        }

        private void OnCommandProcessed(ConsoleOutputData data, string originalInputText)
        {
            DisplayNewOutputEntry(data);
            if (originalInputText != "")
                _commandsHistory.Add(originalInputText);
            inputField.text = "";
        }

        public void DisplayNewOutputEntry(ConsoleOutputData data, bool isFromSpeaker = false)
        {
            var spawnedOutputEntry = Instantiate(outputEntryPrefab, outputContainer);
            ((RectTransform)spawnedOutputEntry.transform).pivot = new Vector2(0, 1);
            spawnedOutputEntry.Init(data, isFromSpeaker);
            if (outputContainer.childCount > maxOutputEntries)
                Destroy(outputContainer.GetChild(0).gameObject);
            outputScrollRect.verticalNormalizedPosition = 0;
            Invoke(nameof(ToggleUI), .05f);
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
            const string initMessage = "summon game";
            var delays = new[] { .25f, .25f, .1f, .35f, .3f, .7f, .15f, .25f, .28f, .2f, .2f };
            inputField.ActivateInputField();
            inputField.Select();
            _animationInputText = "";
            yield return new WaitForSeconds(2.5f);
            for (var i = 0; i < initMessage.Length; i++)
            {
                _soundManager.PlayTypeSound();
                _animationInputText = initMessage.Take(i + 1).Aggregate("", (current, c) => current + c);
                yield return new WaitForSeconds(delays[i]);
            }

            yield return new WaitForSeconds(2.5f);
            _animationInputText = null;
            OnCommandSubmit(initMessage);
        }
    }
}