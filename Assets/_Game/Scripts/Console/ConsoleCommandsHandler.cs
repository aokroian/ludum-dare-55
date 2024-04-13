using System;
using System.Collections.Generic;
using System.Linq;
using static UnityEngine.Object;

namespace _Game.Scripts.Console
{
    public class ConsoleCommandsHandler
    {
        private readonly ConsoleCommand[] _allCommands = ConsoleHelpers.GetAllCommands();
        private readonly List<ConsoleCommand> _knownCommands = new();

        public ConsoleCommandsHandler()
        {
            LoadKnownCommands();
        }

        private ConsoleCommand GenerateCommandFromInput(string input)
        {
            input = input.Trim();
            var inputParts = input.Split(' ');
            if (inputParts.Length != 2)
                return null;
            var summonWord = inputParts[0];
            if (summonWord != "summon")
                return null;

            var commandWord = inputParts[1];
            var command = _allCommands.FirstOrDefault(c =>
                c.mainWord == commandWord || c.aliases.Contains(commandWord));

            return command != null ? Instantiate(command) : null;
        }

        private void SaveKnownCommands()
        {
            // todo: save known commands to player prefs later
        }

        private void LoadKnownCommands()
        {
            // todo: load known commands from player prefs later
            _knownCommands.Clear();
            foreach (var command in _allCommands)
            {
                var knownCommand = Instantiate(command);
                _knownCommands.Add(knownCommand);
            }
        }

        public void SubmitCommand(string inputText, Action<ConsoleCommand, string> onSuccess, Action<string> onFail)
        {
            var command = GenerateCommandFromInput(inputText);
            if (command == null)
            {
                onFail?.Invoke("Unknown command");
                return;
            }

            if (!_knownCommands.IsContainsCommand(command))
            {
                _knownCommands.Add(Instantiate(command));
                SaveKnownCommands();
            }

            // todo: implement command execution later
            onSuccess?.Invoke(command, $"summon {command.mainWord} command executed successfully");
        }
    }
}