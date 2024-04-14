using System;
using System.Collections.Generic;
using Actors.InputThings;
using Actors.Upgrades;
using Common;
using Scene;
using Sounds;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Upgrades
{
    public class UpgradeSystemUI : SingletonScene<UpgradeSystemUI>, Initializable
    {
        [SerializeField] private GameObject upgradePanel;
        [SerializeField] private RectTransform upgradeItemsContainer;
        [SerializeField] private UpgradeItemUI upgradeItemPrefab;

        private ActorStatsSo[] _upgrades;

        private ActorStatsController _playerStatsController;
        private GoLifecycleEventEmitter _upgradePanelLifecycleEvents;

        protected override void Awake()
        {
            base.Awake();
            _upgradePanelLifecycleEvents = upgradePanel.AddComponent<GoLifecycleEventEmitter>();
            _upgradePanelLifecycleEvents.OnEnableEvent += () => { Cursor.visible = true; };
            _upgradePanelLifecycleEvents.OnDisableEvent += () => { Cursor.visible = false; };
        }

        public void Initialize()
        {
            _upgrades = Resources.LoadAll<ActorStatsSo>("Upgrades");
            var player = FindObjectOfType<PlayerActorInput>();
            _playerStatsController = player.GetComponent<ActorStatsController>();
        }

        public void ShowUpgradeSelection(Action<ActorStatsSo> callback)
        {
            ClearContainer();

            var selectedUpgrades = new List<ActorStatsSo>();
            while (selectedUpgrades.Count < 3)
            {
                var upgrade = _upgrades[Random.Range(0, _upgrades.Length)];
                if (selectedUpgrades.Contains(upgrade))
                    continue;

                selectedUpgrades.Add(upgrade);
                Instantiate(upgradeItemPrefab, upgradeItemsContainer)
                    .Initialize(upgrade, upgrade => OnUpgradeSelected(upgrade, callback));
            }

            upgradePanel.SetActive(true);
        }

        private void ClearContainer()
        {
            foreach (Transform child in upgradeItemsContainer)
            {
                Destroy(child.gameObject);
            }
        }

        private void OnUpgradeSelected(ActorStatsSo upgrade, Action<ActorStatsSo> callback)
        {
            SoundSystem.PlayUpgradeButtonClickSound();
            upgradePanel.SetActive(false);
            _playerStatsController.ModifyCurrentStats(upgrade);
            callback?.Invoke(upgrade);
        }
    }
}