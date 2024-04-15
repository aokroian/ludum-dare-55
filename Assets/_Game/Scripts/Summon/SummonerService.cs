using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Game.Scripts.Common;
using _Game.Scripts.Map;
using _Game.Scripts.Summon.View;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Summon
{
    public class SummonerService
    {
        private Summoner.SummonParams _summonParams;
        private GlobalInputSwitcher _inputEnabledHandler;
        private Dictionary<string, Summoner> _summoners;
        
        [Inject] private SoundManager _soundManager;
        [Inject] private CameraWrapper _cameraWrapper;

        public void Init(IEnumerable<Summoner> summoners, GlobalInputSwitcher inputEnabledHandler)
        {
            _inputEnabledHandler = inputEnabledHandler;
            Debug.Log("Summoners amount: " + summoners.Count());
            _summoners = summoners.ToDictionary(it => it.Id);
            foreach (var summoner in summoners)
            {
                summoner.OnSummoned += OnSummonEffects;
            }
        }

        public string Summon(string summonId)
        {
            if (!_summoners.TryGetValue(summonId, out var summoner))
            {
                Debug.LogWarning($"No summoner found for command: {summonId}");
                return "No summoner found for command.";
            }

            var result = summoner.Validate(GetSummonParams());

            _inputEnabledHandler.DisableAllInput();

            var task = summoner.Summon(GetSummonParams());
            var startCameraPosition = GetSummonParams().camera.transform.position;
            task.GetAwaiter().OnCompleted(() => SummonCompleted(task, startCameraPosition));

            return result;
        }

        private void OnSummonEffects(SummonedObject summonedObject)
        {
            _soundManager.PlayUniversalSummonSound();
        }

        private void SummonCompleted(Task task, Vector3 cameraPosition)
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
                GetSummonParams().camera.transform.position = cameraPosition;
            }
            else if (task.IsCanceled)
            {
                Debug.LogWarning("Summon canceled");
            }

            _inputEnabledHandler.EnableAllInput();
        }

        private Summoner.SummonParams GetSummonParams()
        {
            if (_summonParams.camera == null)
                _summonParams = new Summoner.SummonParams(_cameraWrapper);

            return _summonParams;
        }
    }
}