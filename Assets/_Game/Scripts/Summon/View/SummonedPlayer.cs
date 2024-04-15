using System.Linq;
using _Game.Scripts.CharacterRelated._LD55;
using _Game.Scripts.CharacterRelated.Actors.ActorSystems;
using _Game.Scripts.CharacterRelated.UI.Hud;
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
                return new PlayerInSpaceGameplayEvent(this, MessageService);
            }

            if (ObjectsHolder.GetPlayerRoomIndex() == -1)
            {
                return new PlayerInSpaceGameplayEvent(this, MessageService);
            }

            if (ObjectsHolder.GetPlayerRoomOrFirst().Objects.Count(it => it is SummonedPlayer) > 1)
            {
                return new ManyPlayersGameplayEvent(this, MessageService);
            }

            return null;
        }
    }
}