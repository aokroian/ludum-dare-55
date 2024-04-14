using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Summon;
using UnityEngine;
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
            ConsoleOutputData outputData;
            try
            {
                outputData = SubmitCommandUnSafe(inputText, onProcessed);
            }
            catch (Exception e)
            {
                outputData = new ConsoleOutputData
                {
                    senderText = "game@ld-55:$ ",
                    messageText = $"Unknown error: {e.Message}",
                    type = ConsoleOutputType.Error
                };

                Debug.LogError(e);
            }

            onProcessed?.Invoke(outputData, inputText);
        }

        private ConsoleOutputData SubmitCommandUnSafe(string inputText, Action<ConsoleOutputData, string> onProcessed)
        {
            if (inputText is "help" or "?")
                inputText = "summon help";

            var command = GenerateCommandFromInput(inputText);

            if (command == null)
            {
                return new ConsoleOutputData
                {
                    senderText = "game@ld-55:$ ",
                    messageText = "Invalid command format. Try summoning help",
                    type = ConsoleOutputType.Info
                };
            }

            var isHelpCommand = command.mainWord == "help";
            if (!_knownCommands.IsContainsCommand(command))
            {
                _knownCommands.Add(Instantiate(command));
                SaveKnownCommands();
            }

            var outputData = new ConsoleOutputData
            {
                senderText = "game@ld-55:$ ",
                messageText = $"summon {command.mainWord} command executed successfully",
                type = ConsoleOutputType.Info
            };

            if (isHelpCommand)
            {
                outputData.messageText = "Available commands:\n";
                foreach (var knownCommand in _knownCommands)
                {
                    outputData.messageText += $"summon {knownCommand.mainWord}\n";
                }
            }
            else
            {
                var summonResponse = _summonerService.Summon(command.mainWord);
                if (summonResponse != null)
                    outputData.messageText = summonResponse;
            }


            return outputData;
        }
    }
}