using _Game.Scripts.GameState.Data;
using UnityEngine;

namespace _Game.Scripts.GameState
{
    public class GameStatePlayerPrefsProvider: IGameStateProvider
    {
        public PermanentGameState PermanentGameState { get; private set; }
        public TransientGameState TransientGameState { get; private set; }
        
        private const string Key = "PermanentGameState";

        public GameStatePlayerPrefsProvider(PermanentGameState permanentGameState, TransientGameState transientGameState)
        {
            PermanentGameState = permanentGameState;
            TransientGameState = transientGameState;
        }

        public void Save()
        {
            PlayerPrefs.SetString(Key, JsonUtility.ToJson(PermanentGameState));
        }

        public void Load()
        {
            if (PlayerPrefs.HasKey(Key))
            {
                PermanentGameState = JsonUtility.FromJson<PermanentGameState>(PlayerPrefs.GetString(Key));
            }
            else
            {
                PermanentGameState = CreateNewState();
            }
        }
        
        private PermanentGameState CreateNewState()
        {
            return new PermanentGameState();
        }
    }
}