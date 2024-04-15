using System;
using UnityEngine;

namespace _Game.Scripts.CharacterRelated.Actors.Upgrades
{
    [CreateAssetMenu(fileName = "ActorStats", menuName = "LD53/ActorStats", order = 4)]
    public class ActorStatsSo : ScriptableObject
    {
        public event Action OnValidateEvent;

        private void OnValidate()
        {
            OnValidateEvent?.Invoke();
        }

        [Header("Physics")]
        public float addedMovementSpeed;
        public float addedScaleModifier;

        [Header("Health")]
        public int addedMaxHealth;

        [Header("Combat")]
        public float addedShootRate;
        public float addedBulletsSpeed;
        public int addedBulletsDamage;
        public float addedBulletsScale;
        public int addedBulletsPerShotCount;
        public int addedBulletsPiercingCount;

        [Header("Others")]
        public Sprite icon;
        [TextArea]
        public string description;


        public ActorStatsSo GetCopy()
        {
            var copy = CreateInstance<ActorStatsSo>();
            copy.name = name + "_Copy";
            copy.addedMovementSpeed = addedMovementSpeed;
            copy.addedScaleModifier = addedScaleModifier;
            copy.addedMaxHealth = addedMaxHealth;
            copy.addedShootRate = addedShootRate;
            copy.addedBulletsSpeed = addedBulletsSpeed;
            copy.addedBulletsDamage = addedBulletsDamage;
            copy.addedBulletsScale = addedBulletsScale;
            copy.addedBulletsPerShotCount = addedBulletsPerShotCount;
            copy.addedBulletsPiercingCount = addedBulletsPiercingCount;
            copy.icon = icon;
            copy.description = description;
            return copy;
        }
    }
}