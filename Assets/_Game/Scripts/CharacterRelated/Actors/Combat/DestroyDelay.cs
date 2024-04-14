using UnityEngine;
using UnityEngine.Serialization;

namespace Actors.Combat
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