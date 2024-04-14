using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Console
{
    public class ConsoleOutputEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI senderText;
        [SerializeField] private TMP_InputField messageText;
        [SerializeField] private TextMeshProUGUI messageTmp;
        [SerializeField] private LayoutGroup layoutGroup;
        [Space]
        [SerializeField] private Color infoColor;
        [SerializeField] private Color warningColor;
        [SerializeField] private Color errorColor;

        public void Init(ConsoleOutputData data)
        {
            senderText.text = data.senderText;
            messageText.text = data.messageText;

            messageTmp.color = data.type switch
            {
                ConsoleOutputType.Info => infoColor,
                ConsoleOutputType.Warning => warningColor,
                ConsoleOutputType.Error => errorColor,
                _ => messageTmp.color
            };
            senderText.color = data.type switch
            {
                ConsoleOutputType.Info => infoColor,
                ConsoleOutputType.Warning => warningColor,
                ConsoleOutputType.Error => errorColor,
                _ => senderText.color
            };

            layoutGroup.enabled = false;
            layoutGroup.enabled = true;
        }
    }
}