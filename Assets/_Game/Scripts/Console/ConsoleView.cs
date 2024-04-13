using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Console
{
    public class ConsoleView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [Space]
        [SerializeField] private List<ConsoleCommand> submittedCommands = new();
        [SerializeField] private List<ConsoleCommand> knownCommands = new();

        private ConsoleCommand[] _allCommands;
        public event Action<ConsoleCommand> OnCommandSubmitted;
        private ConsoleCommandsHandler _commandsHandler;

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            _commandsHandler = new ConsoleCommandsHandler();
            _commandsHandler.Init(this);
            _allCommands = ConsoleHelpers.GetAllCommands();
            LoadKnownCommands();
            submittedCommands.Clear();

            inputField.onSubmit.RemoveAllListeners();
            inputField.onSubmit.AddListener(OnCommandSubmit);

            inputField.onEndEdit.RemoveAllListeners();
            inputField.onEndEdit.AddListener(FormatInputField);
        }

        private void FormatInputField(string inputText)
        {
            // todo: implement input field text formatting later
        }

        private ConsoleCommand GenerateCommandFromInput()
        {
            var input = inputField.text;
            var inputParts = input.Split(' ');
            var summonWord = inputParts[0];
            if (summonWord != "summon")
                return null;
            var commandWord = inputParts[1];
            var command = _allCommands.FirstOrDefault(c =>
                c.mainWord == commandWord || c.aliases.Contains(commandWord));
            return Instantiate(command);
        }

        private void LoadKnownCommands()
        {
            // todo: load known commands from player prefs later
            knownCommands.Clear();
            foreach (var command in _allCommands)
            {
                var knownCommand = Instantiate(command);
                knownCommands.Add(knownCommand);
            }
        }

        private void SaveKnownCommands()
        {
            // todo: save known commands to player prefs later
        }

        private void OnCommandSubmit(string text)
        {
            var command = GenerateCommandFromInput();
            if (command == null)
                return;
            submittedCommands.Add(Instantiate(command));

            if (!knownCommands.IsContainsCommand(command))
            {
                knownCommands.Add(Instantiate(command));
                SaveKnownCommands();
            }

            OnCommandSubmitted?.Invoke(command);
        }
    }
}