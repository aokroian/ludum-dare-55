using UnityEngine;

namespace _Game.Scripts.CharacterRelated.Actors.Combat
{
    public class DestroyDelay : MonoBehaviour
    {
        [SerializeField] [Range(0f, 10f)] private float timeLife = 2f;
        
        void Start()
        {
            Destroy(gameObject, timeLife);
        }
    }
}