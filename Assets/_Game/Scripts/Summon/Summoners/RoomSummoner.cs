using System.Threading.Tasks;
using UnityEngine;

namespace _Game.Scripts.Summon.Summoners
{
    public class RoomSummoner: Summoner
    {
        [SerializeField] public Transform roomParent;
        
        public override async Task SummonAsync(SummonParams summonParams)
        {
            SummonRoom(summonParams);
        }

        public void SummonRoom(SummonParams summonParams)
        {
            var room = Instantiate(prefabs[0], roomParent);
            room.transform.position = Vector3.zero;
        }
    }
}