using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Game.Scripts.GameLoop;
using UnityEngine;

namespace _Game.Scripts.Summon
{
    public class SummonerService
    {
        private Summoner.SummonParams _summonParams;
        private InputEnabledHandler _inputEnabledHandler;
        private Dictionary<string, Summoner> _summoners;

        public void Init(IEnumerable<Summoner> summoners, InputEnabledHandler inputEnabledHandler)
        {
            _inputEnabledHandler = inputEnabledHandler;
            Debug.Log("Summoners amount: " + summoners.Count());
            _summoners = summoners.ToDictionary(it => it.Id);
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
            var startCameraPosition = GetSummonParams()._camera.transform.position;
            task.GetAwaiter().OnCompleted(() => SummonCompleted(task, startCameraPosition));

            return result;
        }

        private void SummonCompleted(Task task, Vector3 cameraPosition)
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
                GetSummonParams()._camera.transform.position = cameraPosition;
            }
            else if (task.IsCanceled)
            {
                Debug.LogWarning("Summon canceled");
            }
            
            _inputEnabledHandler.EnableAllInput();
        }
        
        private Summoner.SummonParams GetSummonParams()
        {
            if (_summonParams._camera == null)
                _summonParams = new Summoner.SummonParams(Camera.main);

            return _summonParams;
        }
    }
}