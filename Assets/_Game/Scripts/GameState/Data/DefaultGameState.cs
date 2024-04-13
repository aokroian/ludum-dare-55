using UnityEngine;

namespace _Game.Scripts.GameState.Data
{
    [CreateAssetMenu(menuName = "Custom/DefaultGameState")]
    public class DefaultGameState: ScriptableObject
    {
        [SerializeField] private PermanentGameState PermanentGameState;
        [SerializeField] private TransientGameState TransientGameState;
        
        public PermanentGameState GetClonedPermanentGameState()
        {
            return JsonUtility.FromJson<PermanentGameState>(JsonUtility.ToJson(PermanentGameState));
        }
        
        public TransientGameState GetClonedTransientGameState()
        {
            return JsonUtility.FromJson<TransientGameState>(JsonUtility.ToJson(TransientGameState));
        }
    }
}