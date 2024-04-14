using System.Threading.Tasks;
using _Game.Scripts.Message;
using _Game.Scripts.Summon.View;
using Zenject;

namespace _Game.Scripts.Story.GameplayEvents
{
    public class ManyPlayersGameplayEvent: IGameplayEvent
    {
        public string EventId => "many_players_event";

        private SummonedPlayer _player;
        private MessageService _messageService;

        public ManyPlayersGameplayEvent(SummonedPlayer player, MessageService messageService)
        {
            _player = player;
            _messageService = messageService;
        }

        public async Task<string> StartEvent(DiContainer diContainer)
        {
            _messageService.Speak(_player, "Woah what the hell!");
            await Task.Delay(500);

            return "manyPlayers";
        }
    }
}