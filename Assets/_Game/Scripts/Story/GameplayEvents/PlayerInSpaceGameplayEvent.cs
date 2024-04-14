using System.Threading.Tasks;
using _Game.Scripts.Message;
using _Game.Scripts.Summon.View;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Story.GameplayEvents
{
    public class PlayerInSpaceGameplayEvent: IGameplayEvent
    {
        public string EventId => "player_in_space_event";
        private SummonedPlayer _player;
        private MessageService _messageService;


        public PlayerInSpaceGameplayEvent(SummonedPlayer player, MessageService messageService)
        {
            _messageService = messageService;
            _player = player;
        }

        public async Task<string> StartEvent(DiContainer diContainer)
        {
            _messageService.Speak(_player, "Where am I?");
            await _player.transform.DORotate(new Vector3(0, 0, Random.Range(45, 120)), 1f).AsyncWaitForCompletion();
            
            return "space";
        }
    }
}