using System;
using System.Linq;
using UnityEngine;

namespace _Game.Scripts.CharacterRelated.Actors.Combat
{
    [CreateAssetMenu(fileName = "GunsConfig", menuName = "LD53/GunsConfig", order = 2)]
    public class GunsConfigSo : ScriptableObject
    {
        public GunTypesAndPrefabsBinding[] gunBindings;

        public Gun GetGunPrefab(GunTypes gunType)
        {
            return gunBindings.FirstOrDefault(g => g.gunType == gunType)?.gunPrefab;
        }
        public GameObject GetScopePrefab(GunTypes gunType)
        {
            return gunBindings.FirstOrDefault(g => g.gunType == gunType)?.scopePrefab;
        }
    }

    [Serializable]
    public class GunTypesAndPrefabsBinding
    {
        public GunTypes gunType;
        public Gun gunPrefab;
        public GameObject scopePrefab;
    }
}