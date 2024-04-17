using _Game.Scripts.GameState.Data;

namespace _Game.Scripts.Common
{
    public static class ConsoleStrings
    {
        public const string YouMainInputSenderText = "you@ld55:<cspace=.3em><voffset=-.3em>~</voffset>$";
        public const string DevsMainInputSenderText = "devs@ld55:<cspace=.3em><voffset=-.3em>~</voffset>$";

        public static readonly string[] HelpCommandVariants =
            { "help", "summon help", "?", "h", };

        public const string SummonWord = "summon";
        public const string GameRestartCommand = "summon game";
        public const string GameSpeakerName = "game";
        public const string DevsSpeakerName = "devs";

        public const string UnknownErrorMessage = "Unknown error: ";

        public static string GetDiscoveredCommandsMessage(PermanentGameState permanentGameState)
        {
            var result = "";
            result = "Discovered commands so far:\n";
            for (var i = 0; i < permanentGameState.knownCommands.Count; i++)
            {
                var knownCommand = permanentGameState.knownCommands[i];
                if (i != 0)
                    result += "\n";
                result += $"{SummonWord} {knownCommand}";
            }

            return result;
        }

        public static string GetHintAfterGameEndedMessage()
        {
            return "In a situation like this, " +
                   $"\nthe only thing you can do is {"summon game".WrapInColor(Colors.KeywordMessageColor)}";
        }

        public static string GetHintAboutSummoningRoomMessage()
        {
            return
                $"Could use a {(SummonWord + " room").WrapInColor(Colors.KeywordMessageColor)} to make room to move around.";
        }

        public static string GetSpeakerString(string senderName, string senderColor = null)
        {
            return $"[{senderName}]: ".WrapInColor(senderColor);
        }

        public static string GetInvalidCommandMessage(string command)
        {
            return
                $"Invalid command: \"{command}\"\nTry {"summon help".WrapInColor(Colors.KeywordMessageColor)} or something...";
        }

        public static string GetCommandExecutedMessage(string command)
        {
            return $"\"{command}\" command executed";
        }
    }
}