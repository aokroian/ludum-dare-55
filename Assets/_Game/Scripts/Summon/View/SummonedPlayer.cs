using System.Collections.Generic;
using _Game.Scripts.CharacterRelated._LD55;
using _Game.Scripts.CharacterRelated.Actors.ActorSystems;
using _Game.Scripts.CharacterRelated.UI.Hud;
using _Game.Scripts.Common;
using _Game.Scripts.Story;
using _Game.Scripts.Story.GameplayEvents;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Summon.View
{
    public class SummonedPlayer : SummonedObject
    {
        [SerializeField] private Canvas hudCanvas;
        private bool _died;
        [Inject]
        private SignalBus _signalBus;
        [Inject]
        private PlayerInventoryService _playerInventoryService;

        private int _triggeredEventRoomIndex;
        private bool _objectiveSaid;

        protected override void Start()
        {
            base.Start();
            GetComponent<ActorHealth>().OnDeath += _ => OnDeath();
            GetComponentInChildren<PlayerHud>().Init(_signalBus, _playerInventoryService);
            var uiCamera = GameObject.FindWithTag("UICamera");
            if (uiCamera != null)
            {
                hudCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                hudCanvas.worldCamera = uiCamera.GetComponent<Camera>();
            }
        }

        private void OnDeath()
        {
            _died = true;
        }

        public override IGameplayEvent GetEventIfAny()
        {
            if (_died)
            {
                return new PlayerDiedEvent(this, MessageService, "What a miserable death...");
            }

            if (ObjectsHolder.RealRoomCount == 0)
            {
                return new PlayerInSpaceGameplayEvent(this, MessageService, true);
            }

            var playerRoomIndex = ObjectsHolder.GetPlayerRoomIndex();
            if (playerRoomIndex == -1)
            {
                return new PlayerInSpaceGameplayEvent(this, MessageService, false);
            }

            if (ObjectsHolder.GetObjectsById(Id).Count > 1)
            {
                return new ManyPlayersGameplayEvent(this, MessageService);
            }
            
            if (ObjectsHolder.RealRoomCount > 3)
            {
                return new TextGameplayEvent(PrepareMessage("This is too many rooms"), "tooManyRooms");
            }

            if (!_objectiveSaid)
            {
                _objectiveSaid = true;
                return new TextGameplayEvent(
                    PrepareMessage(
                        $"I must save the {"princess".WrapInColor(Colors.KeywordMessageColor)}! this is my duty!"),
                    null);
            }
            

            return null;
        }
        
        private List<TextGameplayEvent.TextEventParams> PrepareMessage(string message)
        {
            var p = new TextGameplayEvent.TextEventParams()
            {
                disableInput = false,
                Duration = 0,
                Speaker = this,
                Text = message
            };
            return new List<TextGameplayEvent.TextEventParams>() {p};
        }
    }
}