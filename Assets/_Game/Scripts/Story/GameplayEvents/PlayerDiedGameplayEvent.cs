using System.Threading.Tasks;
using _Game.Scripts.Message;
using _Game.Scripts.Summon.View;
using Zenject;

namespace _Game.Scripts.Story.GameplayEvents
{
    public class PlayerDiedGameplayEvent: IGameplayEvent
    {
        public string EventId => "player_died_event";

        private SummonedPlayer _player;
        private MessageService _messageService;
        private string _message;


        public PlayerDiedGameplayEvent(SummonedPlayer player, MessageService messageService, string message)
        {
            _player = player;
            _messageService = messageService;
            _message = message;
        }
        
        // Second ending "Shot"?
        
        public async Task<string> StartEvent(DiContainer diContainer)
        {
            _messageService.Speak(_player, _message);
            
            return "death";
        }
    }
}