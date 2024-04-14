using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud
{
    public class PlayerHeart : MonoBehaviour
    {
        [SerializeField] private Image emptyHeart;
        [SerializeField] private Image fullHeart;
        
        public void SetFill(float fill)
        {
            // emptyHeart.fillAmount = fill;
            fullHeart.fillAmount = fill;
        }
    }
}