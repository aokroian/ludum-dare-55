using System;
using System.Linq;
using _Game.Scripts.Common;
using _Game.Scripts.GameLoop;
using _Game.Scripts.GameState.Data;
using _Game.Scripts.Summon;
using UnityEngine;
using static UnityEngine.Object;

namespace _Game.Scripts.Console
{
    public class ConsoleCommandsHandler
    {
        private readonly ConsoleCommand[] _allCommands = ConsoleHelpers.GetAllCommands();
        private readonly SummonerService _summonerService;
        private GameLoopController _gameLoopController;
        public PermanentGameState PermanentGameState { get; private set; }

        public ConsoleCommandsHandler(SummonerService summonerService)
        {
            _summonerService = summonerService;
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


        public void Inject(PermanentGameState permanentGameState, GameLoopController gameLoopController)
        {
            _gameLoopController = gameLoopController;
            PermanentGameState = permanentGameState;
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
                    senderText = "[game]: ",
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
                    senderText = "[game]: ",
                    messageText =
                        $"Cannot execute command.\nTry {"summon help".WrapInColor(Colors.KeywordMessageColor)} or something...",
                    type = ConsoleOutputType.Info
                };
            }

            var isHelpCommand = command.mainWord == "help";

            var outputData = new ConsoleOutputData
            {
                senderText = "[game]: ",
                messageText = $"{inputText} command executed",
                type = ConsoleOutputType.Info
            };

            var isHelpFromDevs = false;
            if (!isHelpCommand && _gameLoopController.IsGameEnded &&
                inputText != "summon game")
            {
                outputData.senderText = "[devs]: ";
                outputData.messageText = "In a situation like this, " +
                                         $"\nthe only thing you can do is {"summon game".WrapInColor(Colors.KeywordMessageColor)}";
                isHelpFromDevs = true;
            }

            if (!PermanentGameState.knownCommands.IsContainsCommand(command))
            {
                PermanentGameState.knownCommands.Add(command.mainWord);
                SaveKnownCommands();
            }


            if (isHelpCommand)
            {
                outputData.messageText = "Known commands:\n";
                for (var i = 0; i < PermanentGameState.knownCommands.Count; i++)
                {
                    var knownCommand = PermanentGameState.knownCommands[i];
                    if (i != 0)
                        outputData.messageText += "\n";
                    outputData.messageText += $"summon {knownCommand}";
                }
            }
            else if (!isHelpFromDevs)
            {
                var summonResponse = _summonerService.Summon(command.mainWord);
                if (summonResponse != null)
                    outputData.messageText = summonResponse;
            }


            return outputData;
        }
    }
}