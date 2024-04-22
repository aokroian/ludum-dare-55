using System.Collections.Generic;
using _Game.Scripts.CharacterRelated._LD55;
using _Game.Scripts.CharacterRelated.Actors.ActorSystems;
using _Game.Scripts.CharacterRelated.UI.Hud;
using _Game.Scripts.Common;
using _Game.Scripts.Story;
using _Game.Scripts.Story.Events;
using _Game.Scripts.Story.GameplayEvents;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Summon.View
{
    public class SummonedPlayer : SummonedObject
    {
        [SerializeField] private Canvas hudCanvas;
        [field: SerializeField] public Collider2D bardWalkArea;
        private bool _died;
        [Inject]
        private SignalBus _signalBus;
        [Inject]
        private PlayerInventoryService _playerInventoryService;
        [Inject]
        private GlobalInputSwitcher _inputSwitcher;

        private int _triggeredEventRoomIndex;
        private bool _objectiveSaid;
        private NewActorAnimations actorAnimations;

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
            
            actorAnimations = GetComponent<NewActorAnimations>();
        }

        private void OnDeath()
        {
            _died = true;
            _inputSwitcher.DisableAllInput();
            _signalBus.Fire(new PlayerDiedEvent());
        }

        public override IGameplayEvent GetEventIfAny()
        {
            if (_died)
            {
                actorAnimations.SetDead();
                var message = GetComponent<ActorGunSystem>().HasGun()
                    ? "What a miserable death..."
                    : $"If only I had a {"weapon".WrapInColor(Colors.KeywordMessageColor)}...";
                return new PlayerDiedGameplayEvent(this, MessageService, message);
            }

            if (ObjectsHolder.RealRoomCount == 0)
            {
                actorAnimations.SetStayStill();
                return new PlayerInSpaceGameplayEvent(this, MessageService, true);
            }

            var playerRoomIndex = ObjectsHolder.GetPlayerRoomIndex();
            if (playerRoomIndex == -1)
            {
                actorAnimations.SetStayStill();
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
            return new List<TextGameplayEvent.TextEventParams>() { p };
        }
        
        public Vector3 GetRandomPointInsideBardWalkArea()
        {
            Vector2 point;
            do
            {
                var x = Random.Range(bardWalkArea.bounds.min.x, bardWalkArea.bounds.max.x);
                var y = Random.Range(bardWalkArea.bounds.min.y, bardWalkArea.bounds.max.y);
                point = new Vector2(x, y);
            } while (!bardWalkArea.OverlapPoint(point));

            return point;
        }
    }
}