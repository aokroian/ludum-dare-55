using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
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
        [SerializeField] private ConsoleOutputEntry outputEntryPrefab;
        [SerializeField] private int maxOutputEntries = 100;

        [Inject] private ConsoleCommandsHandler _commandsHandler;
        [Inject] private SoundManager _soundManager;

        private readonly List<string> _commandsHistory = new();
        private int _currentHistoryIndex = -1;
        private string _inputBeforeUsingHistory;
        private string _animationInputText;

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
                inputField.interactable = true;
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
            _commandsHandler.SubmitCommand(text, OnCommandProcessed);

            inputField.caretPosition = inputField.text.Length;
            inputField.ActivateInputField();
            inputField.Select();
        }

        private void OnCommandProcessed(ConsoleOutputData data, string originalInputText)
        {
            SpawnOutputEntry(data);
            _commandsHistory.Add(originalInputText);
            inputField.text = "";
        }

        private void SpawnOutputEntry(ConsoleOutputData data)
        {
            var spawnedOutputEntry = Instantiate(outputEntryPrefab, outputContainer);
            ((RectTransform)spawnedOutputEntry.transform).pivot = new Vector2(0, 1);
            spawnedOutputEntry.Init(data);
            if (outputContainer.childCount > maxOutputEntries)
                Destroy(outputContainer.GetChild(0).gameObject);
            outputScrollRect.verticalNormalizedPosition = 0;
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
                inputField.SetTextWithoutNotify(_animationInputText);
                inputField.caretPosition = inputField.text.Length;
                inputField.ActivateInputField();
                inputField.Select();
                return;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (_currentHistoryIndex == -1)
                {
                    _inputBeforeUsingHistory = inputField.text;
                    _currentHistoryIndex = _commandsHistory.Count - 1;
                }
                else if (_currentHistoryIndex > 0)
                    _currentHistoryIndex--;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (_currentHistoryIndex != -1)
                    _currentHistoryIndex++;
                if (_currentHistoryIndex >= _commandsHistory.Count)
                    _currentHistoryIndex = -1;
                if (_currentHistoryIndex == -1)
                    inputField.text = _inputBeforeUsingHistory;
            }

            if (_currentHistoryIndex != -1)
            {
                inputField.SetTextWithoutNotify(_commandsHistory[_currentHistoryIndex]);
                inputField.caretPosition = inputField.text.Length;
                inputField.ActivateInputField();
                inputField.Select();
                return;
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