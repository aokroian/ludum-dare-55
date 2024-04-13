using TMPro;
using UnityEngine;

namespace _Game.Scripts.Console
{
    public enum ConsoleOutputType
    {
        Info,
        Warning,
        Error
    }

    public class ConsoleOutputEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI messageText;
        [Space]
        [SerializeField] private Color infoColor;
        [SerializeField] private Color warningColor;
        [SerializeField] private Color errorColor;
        public ConsoleCommand Command { get; private set; }

        public void Init(string message, ConsoleOutputType type, ConsoleCommand command = null)
        {
            Command = command;
            messageText.text = message;
            messageText.color = type switch
            {
                ConsoleOutputType.Info => infoColor,
                ConsoleOutputType.Warning => warningColor,
                ConsoleOutputType.Error => errorColor,
                _ => messageText.color
            };
        }
    }
}