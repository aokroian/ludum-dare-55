using _Game.Scripts.CharacterRelated.Actors.InputThings.StateMachineThings;
using UnityEngine;

namespace _Game.Scripts.CharacterRelated.Actors.InputThings.AI
{
    public class AIActorInput : MonoBehaviour, IActorInput
    {
        public Vector2 Movement { get; private set; }
        public Vector3 Look { get; private set; }
        public bool Fire { get; private set; }

        protected readonly StateMachine StateMachine = new();
        protected Collider2D WalkArea;

        protected void SetMovement(Vector2 movement)
        {
            Movement = movement;
        }

        protected void SetLook(Vector3 look)
        {
            Look = look;
        }

        protected void SetFire(bool fire)
        {
            Fire = fire;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("AI_Walk_Area"))
                WalkArea = other;
        }
    }
}