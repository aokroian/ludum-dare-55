using System.Threading.Tasks;
using _Game.Scripts.Summon.View;
using UnityEngine;

namespace _Game.Scripts.Summon.Summoners
{
    public class GunSummoner: Summoner
    {
        public override async Task Summon(SummonParams summonParams)
        {
            var gun = _diContainer.InstantiatePrefabForComponent<SummonedGun>(prefabs[0]);
            var room = _objectsHolder.GetPlayerRoomOrFirst();
            gun.transform.position = room.transform.position + Vector3.up * 2;
            room.AddObject(gun);
        }
    }
}