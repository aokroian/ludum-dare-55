using System;
using System.Linq;
using _Game.Scripts.Common;
using _Game.Scripts.GameLoop;
using _Game.Scripts.GameState.Data;
using _Game.Scripts.Summon;
using UnityEngine;
using static _Game.Scripts.Common.ConsoleStrings;
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

        private ConsoleCommand TryParseSummonCommand(string input)
        {
            input = input.Trim();
            var inputParts = input.Split(' ');
            if (inputParts.Length != 2)
                return null;
            var summonWord = inputParts[0];
            if (summonWord != SummonWord)
                return null;

            var commandWord = inputParts[1];
            var command = _allCommands.FirstOrDefault(c =>
                c.mainWord == commandWord || c.aliases.Contains(commandWord));

            return command != null ? Instantiate(command) : null;
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
                    senderText = GetSpeakerString(GameSpeakerName),
                    messageText = UnknownErrorMessage + e.Message,
                    type = ConsoleOutputType.Error
                };

                Debug.LogError(e);
            }

            onProcessed?.Invoke(outputData, inputText);
        }

        private ConsoleOutputData SubmitCommandUnSafe(string inputText, Action<ConsoleOutputData, string> onProcessed)
        {
            if (HelpCommandVariants.Any(m => m == inputText)) // if help command
            {
                return new ConsoleOutputData
                {
                    senderText = GetSpeakerString(GameSpeakerName),
                    messageText = GetHelpMessage(PermanentGameState)
                };
            }

            var parsedCommand = TryParseSummonCommand(inputText);
            if (parsedCommand == null)
            {
                return new ConsoleOutputData
                {
                    senderText = GetSpeakerString(GameSpeakerName),
                    messageText = GetInvalidCommandMessage(inputText)
                };
            }

            if (_gameLoopController.IsGameEnded && inputText != GameRestartCommand)
            {
                return new ConsoleOutputData
                {
                    senderText = GetSpeakerString(DevsSpeakerName),
                    messageText = GetHintAfterGameEndedMessage()
                };
            }

            if (!PermanentGameState.knownCommands.IsContainsCommand(parsedCommand))
                PermanentGameState.knownCommands.Add(parsedCommand.mainWord);

            var outputData = new ConsoleOutputData
            {
                senderText = GetSpeakerString(GameSpeakerName),
                messageText = GetCommandExecutedMessage(inputText),
                type = ConsoleOutputType.Info
            };
            var summonResponse = _summonerService.Summon(parsedCommand.mainWord);
            if (summonResponse != null)
                outputData.messageText = summonResponse;

            return outputData;
        }
    }
}