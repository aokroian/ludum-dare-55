using System;
using System.Linq;
using _Game.Scripts.CharacterRelated.Actors.InputThings.AI;
using UnityEngine;

namespace _Game.Scripts.CharacterRelated.Actors.Spawn
{
    [CreateAssetMenu(fileName = "EnemiesConfig", menuName = "LD53/EnemiesConfig", order = 0)]
    public class EnemiesConfigSo : ScriptableObject
    {
        [field: SerializeField] public EnemyTypeAndPrefabPair[] Enemies { get; private set; }

        public AIActorInput GetEnemyPrefab(EnemyTypes enemyType)
        {
            return Enemies.FirstOrDefault(e => e.enemyType == enemyType)?.enemyPrefab;
        }

        public AIActorInput[] GetEnemiesPrefabsByDifficultyWeight(int difficultyWeight)
        {
            return Enemies.Where(e => e.difficultyWeight == difficultyWeight).Select(e => e.enemyPrefab).ToArray();
        }
        
        public EnemyTypeAndPrefabPair GetRandomLessThenDifficultyWeight(float difficultyWeight, int retries = 0)
        {
            var enemies = Enemies.Where(e => e.difficultyWeight <= difficultyWeight).ToArray();
            if (enemies.Length == 0)
                return null;
            
            var retriesLeft = retries;
            EnemyTypeAndPrefabPair result = null;
            do
            {
                retriesLeft--;
                var randIndex = UnityEngine.Random.Range(0, enemies.Length);
                var curEnemy = enemies[randIndex];
                if (result == null || curEnemy.difficultyWeight > result.difficultyWeight)
                    result = curEnemy;
            } while (retriesLeft >= 0);
            
            return result;
        }
    }

    [Serializable]
    public class EnemyTypeAndPrefabPair
    {
        public EnemyTypes enemyType;
        public AIActorInput enemyPrefab;
        public int difficultyWeight;
    }
}