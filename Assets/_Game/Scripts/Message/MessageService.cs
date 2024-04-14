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
            _consoleView.DisplayNewOutputEntry(new ConsoleOutputData()
            {
                messageText = message,
                senderText = $"{speaker.Id}@ld-55:$ ",
                type = ConsoleOutputType.Info
            });
        }
    }
}