using UnityEngine;

namespace _Game.Scripts.GameState.Data
{
    [CreateAssetMenu(menuName = "Custom/DefaultGameState")]
    public class DefaultGameState: ScriptableObject
    {
        public PermanentGameState PermanentGameState;
        public TransientGameState TransientGameState;
    }
}