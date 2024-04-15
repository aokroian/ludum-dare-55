using System.Threading.Tasks;
using _Game.Scripts.Summon.View;
using UnityEngine;

namespace _Game.Scripts.Summon.Summoners
{
    public class EnemySummoner: Summoner
    {
        public override async Task Summon(SummonParams summonParams)
        {
            for (var i = 0; i < _objectsHolder.Rooms.Count; i++)
            {
                var room = _objectsHolder.Rooms[i];
                var enemyPrefab = GetEnemyForRoom(i);
                var enemy = _diContainer.InstantiatePrefabForComponent<SummonedEnemy>(enemyPrefab);
                enemy.transform.position = room.transform.position + Vector3.up * 2.5f;
                room.AddObject(enemy);
            }
        }

        private SummonedEnemy GetEnemyForRoom(int roomIndex)
        {
            if (roomIndex >= prefabs.Count)
                return prefabs[0] as SummonedEnemy;

            return prefabs[roomIndex] as SummonedEnemy;
        }
    }
}