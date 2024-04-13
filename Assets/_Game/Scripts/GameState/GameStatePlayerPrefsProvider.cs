using _Game.Scripts.GameState.Data;
using UnityEngine;

namespace _Game.Scripts.GameState
{
    public class GameStatePlayerPrefsProvider: IGameStateProvider
    {
        public PermanentGameState PermanentGameState { get; private set; }
        public TransientGameState TransientGameState { get; private set; }
        
        private readonly DefaultGameState _defaultGameState;
        private const string Key = "PermanentGameState";

        public GameStatePlayerPrefsProvider(DefaultGameState defaultGameState)
        {
            _defaultGameState = defaultGameState;
        }

        public void Save()
        {
            PlayerPrefs.SetString(Key, JsonUtility.ToJson(PermanentGameState));
        }

        public void Load()
        {
            if (PlayerPrefs.HasKey(Key))
                PermanentGameState = JsonUtility.FromJson<PermanentGameState>(PlayerPrefs.GetString(Key));
            else
                PermanentGameState = CreateNewPermanentState();
            
            TransientGameState = _defaultGameState.GetClonedTransientGameState();
        }
        
        private PermanentGameState CreateNewPermanentState()
        {
            return _defaultGameState.GetClonedPermanentGameState();
        }
    }
}