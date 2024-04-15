using System;
using _Game.Scripts.CharacterRelated.Actors.Combat;
using UnityEngine;

namespace _Game.Scripts.CharacterRelated.Sounds
{
    [CreateAssetMenu(fileName = "SoundsConfig", menuName = "LD53/SoundsConfig", order = 3)]
    public class SoundsConfig : ScriptableObject
    {
        // music not implemented

        [Header("Music")]
        public AudioClip menuMusic;
        public AudioClip combatMusic;
        public AudioClip peacefulMusic;

        [Header("Actor Sounds")]
        public AudioClip actorDamageSound;
        public AudioClip actorDeathSound;
        public AudioClip actorHealSound;

        [Header("Combat Sounds")]
        public BulletTypeToSoundBinding[] bulletsSounds;
        public GunTypeToSoundBinding[] gunShotsSound;

        // below not implemented

        [Header("Map Related Sounds")]
        public AudioClip moveToAnotherDepthSound;
        public AudioClip doorsOpenSound;
        public AudioClip doorsCloseSound;
        public AudioClip collectableSound;
        public AudioClip deliveryReceivedSound;
        public AudioClip deliverySuccessSound;

        [Header("UI Sounds")]
        public AudioClip menuButtonClickSound;
        public AudioClip upgradeButtonClickSound;
        public AudioClip dialogueSound;
    }

    [Serializable]
    public class BulletTypeToSoundBinding
    {
        public BulletTypes bulletType;
        public AudioClip bulletHitSound;
    }

    [Serializable]
    public class GunTypeToSoundBinding
    {
        public GunTypes gunType;
        public AudioClip shotSound;
    }
}