using Actors.InputThings;
using Map.Runtime;
using UnityEngine;

namespace Map.Transitions
{
    public class FloorExit : MonoBehaviour
    {
        [SerializeField] private bool downstairs = true;
        
        [SerializeField] private int currentLevel;
        [SerializeField] private int toLevel;

        public void Initialize(int currentLevel)
        {
            this.currentLevel = currentLevel;
            this.toLevel = currentLevel + (downstairs ? 1 : -1);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                var player = other.gameObject.GetComponent<PlayerActorInput>();
                FloorController.Instance.TryToEnterLevel(player, toLevel, currentLevel < toLevel, Vector3Int.zero);
            }
        }
    }
}