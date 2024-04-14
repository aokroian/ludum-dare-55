using Actors.ActorSystems;
using Actors.Combat;
using UnityEngine;

namespace Actors
{
    public class ActorScopeSystem : ActorSystem
    {
        private Transform _currentScope;
        private SpriteRenderer _currentScopeSpite;
        private GunTypes _currentGunType;


        private GunsConfigSo _gunsConfig;
        private ActorGunSystem _actorGunSystem;

        private bool _isScopeSpawned;

        protected override void Awake()
        {
            base.Awake();
            _gunsConfig = Resources.Load<GunsConfigSo>("GunsConfig");

            var root = transform.root;
            _actorGunSystem = root.GetComponentInChildren<ActorGunSystem>();
            if (_actorGunSystem != null)
                _actorGunSystem.OnActiveGunChanged += OnGunChanged;
        }

        private void Update()
        {
            if (!_isScopeSpawned)
                return;
            _currentScope.transform.position = ActorInput.Look;

            if (_currentGunType == GunTypes.Shotgun)
            {
                RotationForShotgun();
                ScaleForShotgun();
            }

            SetScopeColor();
        }

        private void OnGunChanged(Gun gun)
        {
            var scopePrefab = _gunsConfig.GetScopePrefab(gun.GunType);
            if (scopePrefab == null)
            {
                Debug.LogError($"Scope with type {gun.GunType} not found");
                return;
            }

            if (_currentScope != null)
                Destroy(_currentScope.gameObject);
            var spawnedScope = Instantiate(scopePrefab, transform);
            _currentScopeSpite = spawnedScope.GetComponentInChildren<SpriteRenderer>();
            _currentScope = spawnedScope.transform;
            _currentGunType = gun.GunType;
            _isScopeSpawned = true;
        }

        private void ScaleForShotgun()
        {
            const float maxScale = 4f;
            const float minScale = .7f;
            const float distanceForMaxScale = 4f;
            var distance = Vector2.Distance(transform.position, _currentScope.transform.position);
            var scale = Mathf.Clamp(distance / distanceForMaxScale, minScale, maxScale);
            _currentScope.transform.localScale = Vector3.one * scale;
        }

        private void RotationForShotgun()
        {
            var direction = _currentScope.transform.position - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            _currentScope.transform.rotation = Quaternion.Euler(0, 0, angle + 270);
        }

        private void SetScopeColor()
        {
            var checkRadius = 0.13f;
            var results = new Collider2D[3];
            var size = Physics2D.OverlapCircleNonAlloc(_currentScope.transform.position, checkRadius, results);
            var isEnemyInRange = false;
            var index = 0;
            for (; index < size; index++)
            {
                var colliderEnemy = results[index];
                if (!colliderEnemy.CompareTag("Enemy")) continue;
                isEnemyInRange = true;
                break;
            }

            _currentScopeSpite.color = isEnemyInRange
                ? Color.red
                : Color.white;
        }
    }
}