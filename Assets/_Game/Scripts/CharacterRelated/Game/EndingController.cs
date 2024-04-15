using _Game.Scripts.CharacterRelated.Common;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.CharacterRelated.Game
{
    public class EndingController : SingletonScene<EndingController>
    {
        [SerializeField] private Image endingPanel;
        
        public void Ending()
        {
            endingPanel.gameObject.SetActive(true);
            endingPanel.color = Color.clear;
            var sequence = DOTween.Sequence(transform);
            sequence.Append(endingPanel.DOColor(Color.black, 0.6f));
            sequence.Append(endingPanel.DOColor(new Color(0.8f, 0.8f, 0.8f), 0.6f));
            sequence.OnComplete(() => DialogueController.Instance.Ending());
        }
    }
}