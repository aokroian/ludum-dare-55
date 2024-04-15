using _Game.Scripts.Summon.View;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Map
{
    public abstract class Interactable: MonoBehaviour
    {
        [field:SerializeField] public string Message { get; private set; }
        
        public bool InteractDisabled { get; protected set; }
        
        public abstract void Interact(SummonedPlayer player);
    }
}