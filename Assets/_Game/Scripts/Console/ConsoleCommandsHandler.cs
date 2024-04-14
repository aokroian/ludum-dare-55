using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Summon;
using static UnityEngine.Object;

namespace _Game.Scripts.Console
{
    public class ConsoleCommandsHandler
    {
        private readonly ConsoleCommand[] _allCommands = ConsoleHelpers.GetAllCommands();
        private readonly List<ConsoleCommand> _knownCommands = new();

        private readonly SummonerService _summonerService;

        public ConsoleCommandsHandler(SummonerService summonerService)
        {
            _summonerService = summonerService;
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

        public void SubmitCommand(string inputText, Action<ConsoleOutputData, string> onProcessed)
        {
            var command = GenerateCommandFromInput(inputText);
            if (command == null)
            {
                onProcessed?.Invoke(new ConsoleOutputData
                {
                    senderText = "game@ld-55:$ ",
                    messageText = "Invalid command format. Try summoning help",
                    type = ConsoleOutputType.Info
                }, inputText);
                return;
            }

            if (!_knownCommands.IsContainsCommand(command))
            {
                _knownCommands.Add(Instantiate(command));
                SaveKnownCommands();
            }

            var summonResponse = _summonerService.Summon(command.mainWord);
            var outputData = new ConsoleOutputData
            {
                senderText = "game@ld-55:$ ",
                messageText = $"summon {command.mainWord} command executed successfully",
                type = ConsoleOutputType.Info
            };

            if (summonResponse != null)
            {
                outputData.messageText = summonResponse;
                return;
            }

            onProcessed?.Invoke(outputData, inputText);
        }
    }
}