using _Game.Scripts.CharacterRelated.Actors.InputThings;
using _Game.Scripts.CharacterRelated.Common;
using _Game.Scripts.CharacterRelated.Map.Runtime;
using UnityEngine;

namespace _Game.Scripts.CharacterRelated.Map.Actors
{
    public class RoomExit : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D exitTrigger;
        [SerializeField] private WallDirection direction;
        [SerializeField] private BoxCollider2D shrinkCollider;
        public WallDirection Direction => direction;

        [SerializeField] private GameObject door;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                var player = other.gameObject.GetComponent<PlayerActorInput>();
                CrawlController.Instance.ExitRoom(player, GetComponentInParent<Room>(), this);
            }
        }
        
        public void CloseDoor()
        {
            exitTrigger.enabled = false;
            door.SetActive(true);
            shrinkCollider.gameObject.SetActive(false);
        }
        
        public void OpenDoor()
        {
            exitTrigger.enabled = true;
            door.SetActive(false);
            shrinkCollider.gameObject.SetActive(true);
        }
        
        public void SetTriggerEnabled(bool enabled)
        {
            exitTrigger.enabled = enabled;
        }
    }
}