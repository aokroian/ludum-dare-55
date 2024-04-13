using System.Threading.Tasks;
using _Game.Scripts.Summon.View;
using UnityEngine;

namespace _Game.Scripts.Summon
{
    public abstract class Summoner: ScriptableObject
    {
        [SerializeField] protected SummonedObject prefab;

        public virtual string Validate(SummonParams summonParams)
        {
            return null;
        }
        
        public abstract Task SummonAsync(SummonParams summonParams);

        
        public struct SummonParams
        {
            public readonly Camera _camera;
            public readonly DungeonService _dungeonService;

            public SummonParams(Camera camera, DungeonService dungeonService)
            {
                _camera = camera;
                _dungeonService = dungeonService;
            }
        }
    }
}