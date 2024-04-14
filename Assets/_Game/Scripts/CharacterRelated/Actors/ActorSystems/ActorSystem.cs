using Actors.InputThings;
using UnityEngine;

namespace Actors.ActorSystems
{
    public class ActorSystem : MonoBehaviour
    {
        public IActorInput ActorInput { get; private set; }

        protected virtual void Awake()
        { 
            ActorInput = GetComponent<IActorInput>();
        }
    }
}