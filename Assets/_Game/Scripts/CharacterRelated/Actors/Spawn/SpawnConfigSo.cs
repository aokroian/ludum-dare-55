using UnityEngine;

namespace Actors.Spawn
{
    [CreateAssetMenu(fileName = "SpawnConfigSo", menuName = "LD53/SpawnConfigSo", order = 10)]
    public class SpawnConfigSo : ScriptableObject
    {
        [field: SerializeField] public float StartCost { get; private set; }
        [field: SerializeField] public float DepthCostMultiplier { get; private set; }
        [field: SerializeField] public float DistanceFromStartMultiplier { get; private set; }
        
    }
}