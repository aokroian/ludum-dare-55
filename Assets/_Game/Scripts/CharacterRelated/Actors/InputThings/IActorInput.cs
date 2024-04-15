using UnityEngine;

namespace _Game.Scripts.CharacterRelated.Actors.InputThings
{
    public interface IActorInput
    {
        public Vector2 Movement { get; }
        public Vector3 Look { get; } 
        
        public bool Fire { get; }
    }
}