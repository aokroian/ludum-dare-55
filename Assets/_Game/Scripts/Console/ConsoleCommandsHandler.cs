using UnityEngine;

namespace _Game.Scripts.Console
{
    public class ConsoleCommandsHandler
    {
        public void Init(ConsoleView consoleView)
        {
            consoleView.OnCommandSubmitted += OnCommandSubmitted;
        }

        private void OnCommandSubmitted(ConsoleCommand command)
        {
            Debug.Log("Command submitted: " + command.mainWord);
        }
    }
}