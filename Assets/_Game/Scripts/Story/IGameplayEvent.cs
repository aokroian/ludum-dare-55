using System.Threading.Tasks;
using Zenject;

namespace _Game.Scripts.Story
{
    public interface IGameplayEvent
    {
        public string EventId { get; }
        
        // Returns ending id if event leads to ending
        public Task<string> StartEvent(DiContainer diContainer);
    }
}