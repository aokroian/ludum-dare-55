using _Game.Scripts.Common;
using _Game.Scripts.Console;
using _Game.Scripts.Summon.View;

namespace _Game.Scripts.Message
{
    public class MessageService
    {
        private ConsoleView _consoleView;

        public MessageService(ConsoleView consoleView)
        {
            _consoleView = consoleView;
        }

        public void Speak(SummonedObject speaker, string message)
        {
            var speakerColor = Colors.GetSpeakerColor(speaker.Id);
            var senderText = $"[{speaker.Id}]: ".WrapInColor(speakerColor);
            _consoleView.DisplayNewOutputEntry(new ConsoleOutputData()
            {
                messageText = message,
                senderText = senderText,
                type = ConsoleOutputType.Info
            });
        }
    }
}