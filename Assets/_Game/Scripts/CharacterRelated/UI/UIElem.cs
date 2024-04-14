using UnityEngine;

namespace UI
{
    public class UIElem : MonoBehaviour
    {
        public void Show(float effectDuration = 0.2f)
        {
            // TODO: Add effect
            gameObject.SetActive(true);
        }

        public void Hide(float effectDuration = 0.2f)
        {
            // TODO: Add effect
            gameObject.SetActive(false);
        }
    }
}