using _Game.Scripts.Console;

namespace _Game.Scripts.GameLoop
{
    public class InputEnabledHandler
    {
        private ConsoleView _consoleView;

        public void Init(ConsoleView consoleView)
        {
            _consoleView = consoleView;
        }
        
        public void EnableAllInput()
        {
            _consoleView.EnableInput();
        }
        
        public void DisableAllInput()
        {
            _consoleView.DisableInput();
        }
    }
}