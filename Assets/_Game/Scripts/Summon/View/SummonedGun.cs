using _Game.Scripts.CharacterRelated.Actors.ActorSystems;
using _Game.Scripts.CharacterRelated.Actors.Combat;
using _Game.Scripts.Map.Events;
using _Game.Scripts.Story;
using _Game.Scripts.Summon.Data;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Summon.View
{
    public class SummonedGun: SummonedObject
    {
        [Inject]
        private SummonedObjectsHolder _objectsHolder;
        [Inject] private SignalBus _signalBus;
        
        public override IGameplayEvent GetEventIfAny()
        {
            return null;
        }
        
        // TODO: Animate the gun

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _signalBus.Fire(new PlayerWeaponPickupEvent());
                _objectsHolder.GetPlayerRoomOrFirst().RemoveObject(this);
                other.GetComponent<ActorGunSystem>().ChangeActiveGun(GunTypes.Pistol);
                Destroy(gameObject);
            }
        }
    }
}