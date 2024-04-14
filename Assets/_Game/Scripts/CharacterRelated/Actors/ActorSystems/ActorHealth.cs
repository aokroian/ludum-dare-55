using System;
using Actors.Upgrades;
using Sounds;
using UnityEngine;

namespace Actors.ActorSystems
{
    public class ActorHealth : ActorSystem, IActorStatsReceiver
    {
        [SerializeField] private ActorStatsController actorStatsController;
        public float invincibilityTime;
        public bool isDeathSound;
        public bool isDamageSound;
        public bool isHealSound;
        [SerializeField] private int defaultMaxHealth = 30;

        private bool IsDead => _currentHealth <= 0;
        private float _invincibilityEndTime;
        private int _currentMaxHealth;
        private int _currentHealth;

        public event Action<ActorHealth> OnDeath;
        private event Action<ActorHealth> OnRevive;
        public event Action<int, int> OnHealthChanged;
        public event Action<int> OnHeal;
        public event Action<int> OnDamageTaken;

        protected override void Awake()
        {
            base.Awake();
            _currentMaxHealth = defaultMaxHealth;
            _currentHealth = _currentMaxHealth;
            RegisterActorStatsReceiver();
        }

        private void Start()
        {
            // just to notify other systems about initial health
            OnHealthChanged?.Invoke(_currentHealth, _currentMaxHealth);
        }

        private void OnDestroy()
        {
            UnregisterActorStatsReceiver();
        }

        public void RegisterActorStatsReceiver()
        {
            if (actorStatsController != null)
                actorStatsController.AddReceiver(this);
        }

        public void UnregisterActorStatsReceiver()
        {
            if (actorStatsController != null)
                actorStatsController.RemoveReceiver(this);
        }

        public void ReceiveActorStats(ActorStatsSo actorStatsSo)
        {
            _currentMaxHealth = defaultMaxHealth + actorStatsSo.addedMaxHealth;
            OnHealthChanged?.Invoke(_currentHealth, _currentMaxHealth);
        }

        public void Heal(int amount)
        {
            if (amount <= 0)
                return;
            var newHealth = _currentHealth + amount;

            if (IsDead && newHealth > 0)
                OnRevive?.Invoke(this);

            _currentHealth = newHealth;
            if (_currentHealth > _currentMaxHealth)
                _currentHealth = _currentMaxHealth;

            OnHealthChanged?.Invoke(_currentHealth, _currentMaxHealth);
            OnHeal?.Invoke(amount);
            if (isHealSound)
                SoundSystem.ActorHealSound(this);
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0 || _invincibilityEndTime > Time.time)
                return;

            _invincibilityEndTime = Time.time + invincibilityTime;

            _currentHealth -= damage;
            if (_currentHealth < 0)
                _currentHealth = 0;

            if (IsDead)
            {
                if (isDeathSound)
                    SoundSystem.ActorDeathSound(this);
                OnDeath?.Invoke(this);
            }

            OnHealthChanged?.Invoke(_currentHealth, _currentMaxHealth);
            OnDamageTaken?.Invoke(damage);
            if (isDamageSound)
                SoundSystem.ActorDamageSound(this);
        }
    }
}