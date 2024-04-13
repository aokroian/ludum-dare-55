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
        
        [Inject]
        private ConsoleCommandsHandler _commandsHandler;

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

            outputScrollRect.verticalNormalizedPosition = 0;
            foreach (Transform c in outputContainer)
            {
                Destroy(c.gameObject);
            }
        }

        private void OnCommandSubmit(string text)
        {
            inputField.interactable = false;
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
            // todo: implement input field text formatting later
        }
    }
}