using System;
using System.Collections;
using _Game.Scripts.CharacterRelated.Actors.Combat;
using _Game.Scripts.CharacterRelated.Actors.Upgrades;
using _Game.Scripts.Common;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.CharacterRelated.Actors.ActorSystems
{
    public class ActorGunSystem : ActorSystem
    {
        [field: SerializeField] public ActorStatsController ActorStatsController { get; private set; }
        [SerializeField] private GunTypes startGunType = GunTypes.None;
        [SerializeField] private Transform gunPoint;

        [Inject] private SoundManager _soundManager;
        
        public event Action<Gun> OnActiveGunChanged;

        private Gun _currentActiveGun;
        private GunsConfigSo _gunsConfig;

        private bool _isGunSpawned;

        protected override void Awake()
        {
            base.Awake();
            _gunsConfig = Resources.Load<GunsConfigSo>("GunsConfig");
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(.1f);
            ChangeActiveGun(startGunType);
        }

        private void Update()
        {
            if (!_isGunSpawned)
                return;
            _currentActiveGun.transform.position = gunPoint.position;
            RotateGunAroundPlayer();
            if (ActorInput.Fire && _currentActiveGun != null)
                _currentActiveGun.Fire();

            // temp code jus for testing 
            // if (Input.GetKeyDown(KeyCode.Alpha1))
            //     ChangeActiveGun(GunTypes.Pistol);
            // if (Input.GetKeyDown(KeyCode.Alpha2))
            //     ChangeActiveGun(GunTypes.Shotgun);
            // if (Input.GetKeyDown(KeyCode.Alpha3))
            //     ChangeActiveGun(GunTypes.Rifle);
        }

        public Gun ChangeActiveGun(GunTypes gunType)
        {
            if (_currentActiveGun != null && _currentActiveGun.GunType == gunType)
                return _currentActiveGun;
            var gun = _gunsConfig.GetGunPrefab(gunType);
            if (gun == null && gunType != GunTypes.None)
            {
                Debug.LogError($"Gun with type {gunType} not found");
                return null;
            }

            if (_currentActiveGun != null)
                Destroy(_currentActiveGun.gameObject);
            
            if (gunType == GunTypes.None)
            {
                _isGunSpawned = false;
                return null;
            }

            var spawnedGun = Instantiate(gun, transform);
            spawnedGun.Init(this, _soundManager);

            _currentActiveGun = spawnedGun;
            _isGunSpawned = true;
            OnActiveGunChanged?.Invoke(_currentActiveGun);

            return spawnedGun;
        }


        private void RotateGunAroundPlayer()
        {
            var position = transform.position;
            var mousePosition = ActorInput.Look;
            var direction = (mousePosition - position).normalized;

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _currentActiveGun.transform.rotation = Quaternion.Euler(0, 0, angle);
            _currentActiveGun.FlipSprite(angle);
        }

        public bool HasGun()
        {
            return _currentActiveGun != null;
        }
    }
}