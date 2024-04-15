using System.Threading.Tasks;
using _Game.Scripts.Message;
using _Game.Scripts.Summon.View;
using Zenject;

namespace _Game.Scripts.Story.GameplayEvents
{
    public class ManyPrincessesGameplayEvent : IGameplayEvent
    {
        public string EventId => "many_princesses";
        
        private SummonedPrincess _player;
        private MessageService _messageService;

        public ManyPrincessesGameplayEvent(SummonedPrincess player, MessageService messageService)
        {
            _player = player;
            _messageService = messageService;
        }

        public async Task<string> StartEvent(DiContainer diContainer)
        {
            _messageService.Speak(_player, "Hey!");
            _messageService.Speak(_player, "Hey!");
            await Task.Delay(500);

            return "manyPrincesses";
        }

    }
}