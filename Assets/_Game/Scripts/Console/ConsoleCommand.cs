using UnityEngine;

namespace _Game.Scripts.Console
{
    [CreateAssetMenu(fileName = "ConsoleCommand", menuName = "Custom/ConsoleCommand", order = 0)]
    public class ConsoleCommand : ScriptableObject
    {
        public string mainWord;
        public string[] aliases;
    }
}