using _Game.Scripts.GameState.Data;

namespace _Game.Scripts.GameState
{
    public interface IGameStateProvider
    {
        public PermanentGameState PermanentGameState { get; }
        public TransientGameState TransientGameState { get; }
        
        public void Save();
        
        public void Load();
    }
}