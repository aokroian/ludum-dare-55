using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Story.Ending
{
    public class EndingInfoView : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text progressText;
        
        private CanvasGroup _canvasGroup;
        
        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            ResetView();
        }

        public async Task ShowEndingInfo(EndingService.EndingData endingData, string progress)
        {
            titleText.text = endingData.EndingName;
            descriptionText.text = endingData.EndingDescription;
            progressText.text = progress;
            _canvasGroup.alpha = 0;
            await _canvasGroup.DOFade(1, 0.5f).AsyncWaitForCompletion();
        }

        public void ResetView()
        {
            _canvasGroup.alpha = 0;
        }
    }
}