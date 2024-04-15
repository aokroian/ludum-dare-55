using System;
using _Game.Scripts.CharacterRelated.Actors.Upgrades;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.CharacterRelated.Upgrades
{
    public class UpgradeItemUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text description;
        [SerializeField] private Button button;
        
        public void Initialize(ActorStatsSo upgrade, Action<ActorStatsSo> onUpgradeSelected)
        {
            icon.sprite = upgrade.icon;
            description.text = upgrade.description;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onUpgradeSelected(upgrade));
        }
    }
}