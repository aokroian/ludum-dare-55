using System.Threading.Tasks;
using _Game.Scripts.Common;
using _Game.Scripts.GameLoop.Events;
using _Game.Scripts.Summon.View;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Summon.Summoners
{
    public class PlayerSummoner : Summoner
    {
        [Inject]
        private GlobalInputSwitcher _globalInputSwitcher;
        [Inject] 
        private SignalBus _signalBus;

        public override async Task Summon(SummonParams summonParams)
        {
            var player = _diContainer.InstantiatePrefabForComponent<SummonedPlayer>(prefabs[0]);

            var createdPlayer = _objectsHolder.GetPlayer();
            if (createdPlayer != null)
            {
                player.transform.position = createdPlayer.transform.position -
                                            (createdPlayer.transform.position.normalized * 1.2f);
            }
            else
            {
                player.transform.position = _objectsHolder.Rooms[0].transform.position + Vector3.down * 2f;
            }

            _objectsHolder.Rooms[0].AddObject(player);

            _objectsHolder.SetPlayerRef(player);

            Summoned(player);
            _signalBus.Fire(new SummonPlayerEvent());
            _globalInputSwitcher.SwitchToPlayerControls();
        }
    }
}