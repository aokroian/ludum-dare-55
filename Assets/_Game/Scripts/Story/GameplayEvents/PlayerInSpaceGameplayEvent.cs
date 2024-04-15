using System.Threading.Tasks;
using _Game.Scripts.Common;
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
        private bool _spawnedInSpace;


        public PlayerInSpaceGameplayEvent(SummonedPlayer player, MessageService messageService, bool spawnedInSpace)
        {
            _messageService = messageService;
            _player = player;
            _spawnedInSpace = spawnedInSpace;
        }

        public async Task<string> StartEvent(DiContainer diContainer)
        {
            var message = _spawnedInSpace
                ? "Where am I?"
                : $"It would be nice if that door lead to another {"room".WrapInColor(Colors.KeywordMessageColor)}";
            _messageService.Speak(_player, message);
            await _player.transform.DORotate(new Vector3(0, 0, Random.Range(45, 120)), 1f).AsyncWaitForCompletion();
            
            return "space";
        }
    }
}