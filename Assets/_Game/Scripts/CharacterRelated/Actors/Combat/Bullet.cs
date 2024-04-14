using _Game.Scripts;
using Actors.ActorSystems;
using Actors.Upgrades;
using UnityEngine;
using Zenject;

namespace Actors.Combat
{
    public class Bullet : MonoBehaviour, IActorStatsReceiver
    {
        [field: SerializeField] public BulletTypes BulletType { get; private set; }
        [SerializeField] private GameObject particlesPrefab;
        [SerializeField] private float bulletSpeed = 10f;
        [SerializeField] private int bulletDamage;
        [SerializeField] private int bulletPiercingCount;

        private SoundManager _soundManager;

        private Gun _gun;
        private ActorStatsController _actorStatsController;

        private float _defaultBulletScale;
        private float _currentBulletSpeed;
        private int _currentBulletDamage;
        private float _currentBulletScale;
        private float _currentBulletPiercingCount;

        public void Init(Gun gun, SoundManager soundManager)
        {
            _soundManager = soundManager;
            _gun = gun;
            _actorStatsController = gun.ActorStatsController;

            _defaultBulletScale = transform.localScale.x;
            _currentBulletScale = _defaultBulletScale;
            _currentBulletSpeed = bulletSpeed;
            _currentBulletDamage = bulletDamage;
            _currentBulletPiercingCount = bulletPiercingCount;

            RegisterActorStatsReceiver();
        }

        private void OnDestroy()
        {
            UnregisterActorStatsReceiver();
        }

        public void RegisterActorStatsReceiver()
        {
            if (_actorStatsController != null)
                _actorStatsController.AddReceiver(this);
        }

        public void UnregisterActorStatsReceiver()
        {
            if (_actorStatsController != null)
                _actorStatsController.RemoveReceiver(this);
        }

        public void ReceiveActorStats(ActorStatsSo actorStatsSo)
        {
            _currentBulletDamage = bulletDamage + actorStatsSo.addedBulletsDamage;
            _currentBulletSpeed = bulletSpeed + actorStatsSo.addedBulletsSpeed;
            _currentBulletScale = _defaultBulletScale + actorStatsSo.addedBulletsScale;
            _currentBulletPiercingCount = bulletPiercingCount + actorStatsSo.addedBulletsPiercingCount;

            if (_currentBulletPiercingCount < 0)
                _currentBulletPiercingCount = 0;

            transform.localScale = Vector3.one * _currentBulletScale;
        }

        private void Update()
        {
            transform.Translate(Vector2.right * (_currentBulletSpeed * Time.deltaTime));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.IsChildOf(_gun.OwnerActor) ||
                other.CompareTag("AI_Walk_Area") ||
                other.CompareTag("Damage_Collider") ||
                other.CompareTag("Fit_Player_To_Door_Area"))
            {
                return;
            }

            if (other.CompareTag("Enemy") || other.CompareTag("Player"))
            {
                var actorHealth = other.GetComponentInChildren<ActorHealth>();
                if (actorHealth != null)
                    actorHealth.TakeDamage(_currentBulletDamage);
                _soundManager.PlayBulletHitSound(transform.position);
                if (_currentBulletPiercingCount <= 0)
                    Destroy(gameObject);
                _currentBulletPiercingCount--;
            }
            else
                OnHitObstacle(other);
        }

        private void OnHitObstacle(Collider2D other)
        {
            var position = transform.position;
            var hit = Physics2D.Raycast(position, other.transform.position - position);
            Vector3 normal = hit.normal;
            var spawned = Instantiate(particlesPrefab, hit.point, Quaternion.identity);
            spawned.transform.localScale = Vector3.one * _currentBulletScale;
            spawned.transform.forward = normal;
            _soundManager.PlayBulletHitSound(transform.position);
            Destroy(gameObject);
        }
    }
}