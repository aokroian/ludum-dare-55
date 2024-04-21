using System.Threading.Tasks;
using _Game.Scripts.CharacterRelated._LD55.Events;
using _Game.Scripts.Summon.View;
using Zenject;

namespace _Game.Scripts.Summon.Summoners
{
    public class BardSummoner : Summoner
    {
        [Inject]
        private SignalBus _signalBus;

        public override Task Summon(SummonParams summonParams)
        {
            var player = _objectsHolder.GetPlayer();
            var room = player == null ? _objectsHolder.Rooms[0] : player.CurrentRoom;
            var spawned = _diContainer.InstantiatePrefabForComponent<SummonedBard>(prefabs[0]);
            spawned.transform.position = player != null
                ? player.GetRandomPointInsideBardWalkArea()
                : room.GetRandomPointInsideWalkArea();
            room.AddObject(spawned);
            _signalBus.Fire(new BardSummonedEvent(spawned));
            return Task.CompletedTask;
        }
    }
}