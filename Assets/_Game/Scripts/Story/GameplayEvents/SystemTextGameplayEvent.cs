using System.Collections.Generic;
using System.Threading.Tasks;
using _Game.Scripts.Common;
using _Game.Scripts.Message;
using _Game.Scripts.Summon.View;
using Cysharp.Threading.Tasks;
using Zenject;

namespace _Game.Scripts.Story.GameplayEvents
{
    public class SystemTextGameplayEvent: IGameplayEvent
    {
        public string EventId => "system_text_event";

        private string _message;
        private string _endingId;

        public SystemTextGameplayEvent(string message, string endingId)
        {
            _endingId = endingId;
            _message = message;
        }
        
        public async Task<string> StartEvent(DiContainer diContainer)
        {
            var messageService = diContainer.Resolve<MessageService>();
            var inputHandler = diContainer.Resolve<GlobalInputSwitcher>();
            
            messageService.SpeakSystem(_message);
            
            return _endingId;
        }

        public struct TextEventParams
        {
            public string Text;
            public SummonedObject Speaker;
            public float Duration;
            public bool disableInput;
        }
    }
}