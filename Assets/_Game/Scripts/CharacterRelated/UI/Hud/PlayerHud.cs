using System.Collections.Generic;
using _Game.Scripts.CharacterRelated.Actors.ActorSystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.CharacterRelated.UI.Hud
{
    public class PlayerHud : MonoBehaviour
    {
        [SerializeField] private int healthInHeart = 2;
        [SerializeField] private Transform heartsContainer;
        [SerializeField] private GameObject heartPrefab;
        [SerializeField] private GameObject coinsObject;
        [SerializeField] private TextMeshProUGUI coinsCountTmp;
        [SerializeField] private GameObject keysObject;
        [SerializeField] private TextMeshProUGUI keysCountTmp;
        [SerializeField] private LayoutGroup[] layoutGroups;


        private int _curMaxHealth;
        private readonly List<PlayerHeart> _hearts = new();

        private void Awake()
        {
            var playerHealth = GetComponentInParent<ActorHealth>();
            playerHealth.OnHealthChanged += (curHealth, curMaxHealth) => SetHealth(curMaxHealth, curHealth);
        }

        private void RefreshLayoutGroups()
        {
            foreach (var layoutGroup in layoutGroups)
            {
                layoutGroup.enabled = false;
                layoutGroup.enabled = true;
            }
        }

        private void SetCoins(int count)
        {
            coinsObject.SetActive(count > 0);
            coinsCountTmp.text = count.ToString();
            RefreshLayoutGroups();
        }

        private void SetKeys(int count)
        {
            keysObject.SetActive(count > 0);
            keysCountTmp.text = count.ToString();
            RefreshLayoutGroups();
        }

        private void SetHealth(int maxHealth, int health)
        {
            if (_curMaxHealth != maxHealth)
            {
                RecreateHearts(maxHealth / healthInHeart);
            }

            FillHearts(health);
            RefreshLayoutGroups();
            Canvas.ForceUpdateCanvases();
        }

        private void RecreateHearts(int hearts)
        {
            _hearts.Clear();
            foreach (Transform child in heartsContainer)
            {
                Destroy(child.gameObject);
            }

            for (var i = 0; i < hearts; i++)
            {
                var obj = Instantiate(heartPrefab, heartsContainer);
                _hearts.Add(obj.GetComponent<PlayerHeart>());
            }
        }

        private void FillHearts(int health)
        {
            var fullHearts = Mathf.FloorToInt(health / healthInHeart);
            var healthLeft = health % healthInHeart;

            for (var i = 0; i < _hearts.Count; i++)
            {
                var heart = _hearts[i];
                if (i < fullHearts)
                    heart.SetFill(1);
                else if (i == fullHearts)
                    heart.SetFill(healthLeft / (float)healthInHeart);
                else
                    heart.SetFill(0);
            }
        }
    }
}