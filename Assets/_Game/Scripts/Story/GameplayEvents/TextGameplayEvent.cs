using System.Collections.Generic;
using System.Threading.Tasks;
using _Game.Scripts.GameLoop;
using _Game.Scripts.Message;
using _Game.Scripts.Summon.View;
using Unity.VisualScripting;
using Zenject;

namespace _Game.Scripts.Story.GameplayEvents
{
    public class TextGameplayEvent: IGameplayEvent
    {
        public string EventId => "text_event";
        
        private List<TextEventParams> _lines;
        private string _endingId;

        public TextGameplayEvent(List<TextEventParams> lines, string endingId)
        {
            _endingId = endingId;
            _lines = lines;
        }
        
        public async Task<string> StartEvent(DiContainer diContainer)
        {
            var messageService = diContainer.Resolve<MessageService>();
            var inputHandler = diContainer.Resolve<InputEnabledHandler>();
            
            foreach (var line in _lines)
            {
                if (line.disableInput)
                    inputHandler.DisableAllInput();
                messageService.Speak(line.Speaker, line.Text);
                await Task.Delay((int) (line.Duration * 1000));
                if (line.disableInput)
                    inputHandler.EnableAllInput();
            }
            
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