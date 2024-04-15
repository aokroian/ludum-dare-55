using _Game.Scripts.CharacterRelated.Actors.InputThings;
using UnityEngine;

namespace _Game.Scripts.CharacterRelated.Actors.ActorSystems
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