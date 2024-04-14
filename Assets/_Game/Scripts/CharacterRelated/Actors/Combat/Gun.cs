using System;
using _Game.Scripts;
using Actors.ActorSystems;
using Actors.Upgrades;
using UnityEngine;
using Zenject;

namespace Actors.Combat
{
    public class Gun : MonoBehaviour, IActorStatsReceiver
    {
        [field: SerializeField] public GunTypes GunType { get; private set; }
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform bulletSpawnPoint;

        [Space]
        [SerializeField] private float defaultShootRate = 1;
        [SerializeField] private int defaultBulletsPerShotCount = 1;
        [SerializeField] private float zAngleBetweenBullets = 8f;


        public Transform OwnerActor { get; private set; }
        public ActorGunSystem GunSystem { get; private set; }
        public ActorStatsController ActorStatsController { get; private set; }

        private float _shootRateTimer;
        private Vector3 _defaultScale;
        private float _currentShootRate;
        private float _currentBulletsPerShotCount;

        private SoundManager _soundManager;

        public event Action OnFire;

        public void Init(ActorGunSystem actorGunSystem, SoundManager soundManager,
            ActorStatsController actorStatsController = null)
        {
            _soundManager = soundManager;
            OwnerActor = actorGunSystem.gameObject.transform;
            GunSystem = actorGunSystem;
            ActorStatsController = actorStatsController;

            _currentShootRate = defaultShootRate;
            _currentBulletsPerShotCount = defaultBulletsPerShotCount;
            _defaultScale = transform.localScale;
            RegisterActorStatsReceiver();
        }

        private void OnDestroy()
        {
            UnregisterActorStatsReceiver();
        }

        private void Update()
        {
            _shootRateTimer -= Time.deltaTime;
        }

        public void RegisterActorStatsReceiver()
        {
            if (ActorStatsController != null)
                ActorStatsController.AddReceiver(this);
        }

        public void UnregisterActorStatsReceiver()
        {
            if (ActorStatsController != null)
                ActorStatsController.RemoveReceiver(this);
        }

        public void ReceiveActorStats(ActorStatsSo actorStatsSo)
        {
            _currentShootRate = defaultShootRate + actorStatsSo.addedShootRate;
            if (_currentShootRate <= 0)
                _currentShootRate = 0.1f;
            _currentBulletsPerShotCount = defaultBulletsPerShotCount + actorStatsSo.addedBulletsPerShotCount;
            if (_currentBulletsPerShotCount < 1)
                _currentBulletsPerShotCount = 1;
        }

        public void Fire()
        {
            if (_shootRateTimer <= 0)
            {
                var fullAngle = _currentBulletsPerShotCount > 1
                    ? _currentBulletsPerShotCount * zAngleBetweenBullets
                    : 0;

                var initialAngle = -(fullAngle / 2);
                OnFire?.Invoke();
                _soundManager.PlayBulletShotSound(transform.position);
                for (var i = 0; i < _currentBulletsPerShotCount; i++)
                {
                    var bulletRotation = Quaternion.Euler(
                        0,
                        0,
                        transform.rotation.eulerAngles.z + initialAngle + i * zAngleBetweenBullets);
                    var spawnedBullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletRotation);
                    spawnedBullet.Init(this, _soundManager);
                }

                _shootRateTimer = 1 / _currentShootRate;
            }
        }

        public void FlipSprite(float angleZ)
        {
            transform.localScale = angleZ is > 90 or < -90
                ? new Vector3(_defaultScale.x, -_defaultScale.y, _defaultScale.z)
                : _defaultScale;
        }
    }
}