using System.Threading.Tasks;
using _Game.Scripts.Summon.View;

namespace _Game.Scripts.Summon.Summoners
{
    public class PrincessSummoner: Summoner
    {
        public override async Task Summon(SummonParams summonParams)
        {
            var startPosition = summonParams.camera.transform.position;
            var roomIndex = _objectsHolder.Rooms.Count - 1;
            var room = _objectsHolder.Rooms[roomIndex];
            var prefab = GetPrincessPrefab(room);

            var spawnPosition = room.PrincessSpawnPoint != null
                ? room.PrincessSpawnPoint.position
                : room.transform.position;
            
            if (roomIndex > 0)
                await MoveCameraToAsync(summonParams.camera, spawnPosition);
            
            var princess = _diContainer.InstantiatePrefabForComponent<SummonedPrincess>(prefab);
            princess.transform.position = spawnPosition;
            room.AddObject(princess);
            await Task.Delay(300);
            
            if (roomIndex > 0)
                await MoveCameraToAsync(summonParams.camera, startPosition);
        }

        private SummonedPrincess GetPrincessPrefab(SummonedRoom room)
        {
            return prefabs[0] as SummonedPrincess;
        }
    }
}