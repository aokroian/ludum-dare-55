using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Game.Scripts.Console
{
    public class ConsoleView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private ScrollRect outputScrollRect;
        [SerializeField] private Transform outputContainer;
        [SerializeField] private ConsoleOutputEntry outputEntryPrefab;
        [SerializeField] private int maxOutputEntries = 100;

        private readonly List<ConsoleCommand> _executedCommands = new();

        [Inject] private ConsoleCommandsHandler _commandsHandler;
        [Inject] private SoundManager _soundManager;

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            _executedCommands.Clear();
            inputField.onSubmit.RemoveAllListeners();
            inputField.onSubmit.AddListener(OnCommandSubmit);
            inputField.onEndEdit.RemoveAllListeners();
            inputField.onEndEdit.AddListener(FormatInputField);
            inputField.onValueChanged.RemoveAllListeners();
            inputField.onValueChanged.AddListener(OnInputValueChanged);

            outputScrollRect.verticalNormalizedPosition = 0;
            foreach (Transform c in outputContainer)
            {
                Destroy(c.gameObject);
            }

            StartCoroutine(GameInitAnimation());
        }

        private void OnInputValueChanged(string currentInputValue)
        {
            // if (currentInputValue.Length != _previousInputValue.Length)
            // _soundManager.PlayTypeSound();
            // _previousInputValue = currentInputValue;
        }

        private void OnCommandSubmit(string text)
        {
            inputField.interactable = false;
            _soundManager.PlaySubmitKeySound();
            _commandsHandler.SubmitCommand(text, OnCommandSuccess, OnCommandFailed);
        }

        private void OnCommandSuccess(ConsoleCommand command, string message)
        {
            _executedCommands.Add(Instantiate(command));
            SpawnOutputEntry(message, ConsoleOutputType.Info);
            inputField.text = "";
            inputField.interactable = true;
        }

        private void OnCommandFailed(string message)
        {
            SpawnOutputEntry(message, ConsoleOutputType.Error);
            inputField.interactable = true;
        }

        private void SpawnOutputEntry(string message, ConsoleOutputType type)
        {
            var spawnedOutputEntry = Instantiate(outputEntryPrefab, outputContainer);
            spawnedOutputEntry.Init(message, type);
            if (outputContainer.childCount > maxOutputEntries)
                Destroy(outputContainer.GetChild(0).gameObject);
            outputScrollRect.verticalNormalizedPosition = 0;
        }

        private void FormatInputField(string inputText)
        {
            // to lower case
            inputField.text = inputText.ToLower();
        }

        private IEnumerator GameInitAnimation()
        {
            inputField.text = "";
            const string initMessage = "summon game";
            var delays = new[] { .25f, .25f, .1f, .35f, .3f, .7f, .15f, .25f, .28f, .2f, .2f };
            inputField.ActivateInputField();
            inputField.Select();
            yield return new WaitForSeconds(2.5f);
            for (var i = 0; i < initMessage.Length; i++)
            {
                inputField.text += initMessage[i];
                _soundManager.PlayTypeSound();
                inputField.caretPosition = inputField.text.Length;
                yield return new WaitForSeconds(delays[i]);
            }

            inputField.caretPosition = inputField.text.Length;
            inputField.ActivateInputField();
            inputField.Select();
            yield return new WaitForSeconds(2.5f);
            OnCommandSubmit(inputField.text);
        }
    }
}