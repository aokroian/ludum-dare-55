using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Game.Scripts.GameLoop.Events;
using _Game.Scripts.GameState.Data;
using _Game.Scripts.Map;
using _Game.Scripts.Story.Events;
using _Game.Scripts.Summon.Data;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Story.Ending
{
    public class EndingService
    {
        private SignalBus _signalBus;
        private Config _config;
        private CameraWrapper _cameraWrapper;
        private SummonedObjectsHolder _objectsHolder;
        private EndingInfoView _endingInfoView;

        private List<string> _knownEndings;
        private int _allEndingsAmount;

        public EndingService(SignalBus signalBus,
            Config config,
            CameraWrapper cameraWrapper,
            SummonedObjectsHolder objectsHolder,
            EndingInfoView endingInfoView)
        {
            _signalBus = signalBus;
            _config = config;
            _cameraWrapper = cameraWrapper;
            _objectsHolder = objectsHolder;
            _endingInfoView = endingInfoView;
            
            _signalBus.Subscribe<GameStartEvent>(OnGameStart);
            _allEndingsAmount = _config.endings.Count;
        }

        public void Init(PermanentGameState permanentGameState)
        {
            if (permanentGameState.knownEndings == null)
                permanentGameState.knownEndings = new List<string>();
            _knownEndings = permanentGameState.knownEndings;
        }

        public async Task ShowEnding(string endingId)
        {
            var ending = _config.endings.Find(e => e.EndingId == endingId);
            SetEndingAsKnown(ending.EndingId);
            
            _signalBus.Fire(new EndingStartedEvent(ending));
            Debug.LogWarning("Ending " + endingId);
            await ShowEndingAnimation(ending);
            await ShowEndingInfo(ending);
            _signalBus.Fire(new GameEndEvent(endingId));
        }

        private async Task ShowEndingAnimation(EndingData endingData)
        {
            if (string.IsNullOrWhiteSpace(endingData.CustomObjectId))
            {
                var target = _objectsHolder.GetPlayer()?.transform.position ?? Vector3.zero;
                await _cameraWrapper.ZoomIn(target);
            }
            else
            {
                var target = _objectsHolder.GetObjectById(endingData.CustomObjectId)?.transform.position ??
                             Vector3.zero;
                await _cameraWrapper.ZoomIn(target);
            }
        }

        private async Task ShowEndingInfo(EndingData endingData)
        {
            await _endingInfoView.ShowEndingInfo(endingData, $"{_knownEndings.Count}/{_allEndingsAmount} endings found");
        }
        
        private void SetEndingAsKnown(string endingId)
        {
            if (!_knownEndings.Contains(endingId))
                _knownEndings.Add(endingId);
        }

        private void OnGameStart()
        {
            _endingInfoView.ResetView();
        }
        
        [Serializable]
        public class Config
        {
            public List<EndingData> endings;
        }

        [Serializable]
        public class EndingData
        {
            public string EndingId;
            public string EndingName;
            public string EndingDescription;
            public bool IsGoodEnding;
            public string CustomObjectId;
        }
    }
}