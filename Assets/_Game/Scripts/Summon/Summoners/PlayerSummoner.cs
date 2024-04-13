using System.Threading.Tasks;
using UnityEngine;

namespace _Game.Scripts.Summon.Summoners
{
    [CreateAssetMenu(menuName = "Custom/Summon/Summoners/PlayerSummoner", fileName = "PlayerSummoner")]
    public class PlayerSummoner: Summoner
    {
        public override async Task SummonAsync(SummonParams summonParams)
        {
            
        }
    }
}