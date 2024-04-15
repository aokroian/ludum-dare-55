using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.CharacterRelated.UI.Dialogue
{
    public class DialoguePanel : MonoBehaviour
    {
        [SerializeField] Image portrait;
        [SerializeField] TMP_Text nameText;
        [SerializeField] TMP_Text messageText;
        [SerializeField] private Button confirmBtn;
        [SerializeField] private TMP_Text confirmBtnText;
        [SerializeField] private Button cancelBtn;
        [SerializeField] private TMP_Text cancelBtnText;

        private void OnEnable()
        {
            Cursor.visible = true;
        }

        private void OnDisable()
        {
            Cursor.visible = false;
        }

        public void Show(DialogueConfig config, Action confirmAction = null, Action cancelAction = null)
        {
            ApplyConfig(config);
            
            confirmBtn.onClick.RemoveAllListeners();
            cancelBtn.onClick.RemoveAllListeners();
            confirmBtn.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                confirmAction?.Invoke();
            });
            cancelBtn.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                cancelAction?.Invoke();
            });
        }

        private void ApplyConfig(DialogueConfig config)
        {
            portrait.gameObject.SetActive(config.portrait != null);
            nameText.gameObject.SetActive(config.title != null);
            messageText.gameObject.SetActive(config.message != null);
            confirmBtn.gameObject.SetActive(config.confirmBtnText != null);
            cancelBtn.gameObject.SetActive(config.cancelBtnText != null);
            
            portrait.sprite = config.portrait;
            nameText.text = config.title;
            messageText.text = config.message;
            confirmBtnText.text = config.confirmBtnText;
            cancelBtnText.text = config.cancelBtnText;
            gameObject.SetActive(true);
        }
    }
}