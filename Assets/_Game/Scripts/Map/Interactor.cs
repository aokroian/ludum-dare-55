using _Game.Scripts.Summon.View;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Map
{
    public class Interactor: MonoBehaviour
    {
        [SerializeField] private TMP_Text interactionText;
        
        private Interactable _currentInteractable;

        private SummonedPlayer _player;

        private void Start()
        {
            _player = GetComponent<SummonedPlayer>();
            interactionText.text = "";
        }

        private void Update()
        {
            if ( Input.GetKeyDown(KeyCode.E) && _currentInteractable != null)
            {
                _currentInteractable.Interact(_player);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        { 
            Debug.Log("Collided", other.gameObject);
            if (other.CompareTag("Interactable"))
            {
                var interactable = other.GetComponent<Interactable>();
                if (!interactable.InteractDisabled)
                {
                    _currentInteractable = interactable;
                    interactionText.text = _currentInteractable.Message;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log("ColliderExit", other.gameObject);
            if (other.CompareTag("Interactable"))
            {
                _currentInteractable = null;
                interactionText.text = "";
            }
        }
    }
}