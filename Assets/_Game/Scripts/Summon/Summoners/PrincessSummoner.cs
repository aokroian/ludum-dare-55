using System.Threading.Tasks;
using _Game.Scripts.Summon.View;

namespace _Game.Scripts.Summon.Summoners
{
    public class PrincessSummoner: Summoner
    {
        public override async Task Summon(SummonParams summonParams)
        {
            var startPosition = summonParams.camera.transform.position;
            var roomIndex = _objectsHolder.rooms.Count - 1;
            var room = _objectsHolder.rooms[roomIndex];
            var prefab = GetPrincessPrefab(room);

            var spawnPosition = room.PrincessSpawnPoint != null
                ? room.PrincessSpawnPoint.position
                : room.transform.position;
            await MoveCameraToAsync(summonParams.camera, spawnPosition);
            var princess = _diContainer.InstantiatePrefabForComponent<SummonedPrincess>(prefab);
            princess.transform.position = spawnPosition;
            await Task.Delay(300);
            await MoveCameraToAsync(summonParams.camera, startPosition);
        }

        private SummonedPrincess GetPrincessPrefab(SummonedRoom room)
        {
            return _diContainer.InstantiatePrefabForComponent<SummonedPrincess>(prefabs[0]);
        }
    }
}