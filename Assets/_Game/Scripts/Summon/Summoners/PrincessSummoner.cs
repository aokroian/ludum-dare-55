using System.Threading.Tasks;
using _Game.Scripts.Summon.View;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Game.Scripts.Summon.Summoners
{
    public class PrincessSummoner: Summoner
    {
        public override async Task Summon(SummonParams summonParams)
        {
            try
            {
                await SummonInternal(summonParams);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                
            }
        }

        private async Task SummonInternal(SummonParams summonParams)
        {
            var startPosition = summonParams.camera.transform.position;
            var roomIndex = _objectsHolder.Rooms.Count - 1;
            var room = _objectsHolder.Rooms[roomIndex];
            var prefab = GetPrincessPrefab(room);
            
            var createdPrincess = _objectsHolder.GetPrincess();

            Vector3 spawnPosition;
            if (createdPrincess == null)
            {
                spawnPosition = room.PrincessSpawnPoint != null
                    ? room.PrincessSpawnPoint.position
                    : room.transform.position;
            }
            else
            {
                spawnPosition = createdPrincess.transform.position -
                                (createdPrincess.transform.position.normalized * 1.2f);
            }
            
            if (roomIndex > 0)
                await MoveCameraToAsync(summonParams.camera, spawnPosition);
            
            var princess = _diContainer.InstantiatePrefabForComponent<SummonedPrincess>(prefab);
            princess.transform.position = spawnPosition;
            room.AddObject(princess);
            _objectsHolder.SetPrincessRef(princess);
            
            await UniTask.Delay(300);
            
            if (roomIndex > 0)
                await MoveCameraToAsync(summonParams.camera, startPosition);
        }

        private SummonedPrincess GetPrincessPrefab(SummonedRoom room)
        {
            return prefabs[0] as SummonedPrincess;
        }
    }
}