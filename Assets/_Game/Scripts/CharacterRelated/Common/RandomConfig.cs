using UnityEngine;

namespace _Game.Scripts.CharacterRelated.Common
{
    public class RandomConfig : SingletonGlobal<RandomConfig>
    {
        [SerializeField] private bool randomSeed;
        [SerializeField] private int globalSeed = 12345;

        protected override void Awake()
        {
            base.Awake();
            if (randomSeed)
            {
                globalSeed = Random.Range(0, int.MaxValue);
            }
            Random.InitState(globalSeed);
        }

        public void SetGlobalSeed(int seed)
        {
            this.globalSeed = seed;
            Random.InitState(seed);
        }
        
        public void ResetRandom(int seedPart)
        {
            Random.InitState(globalSeed + seedPart);
        }
    }
}