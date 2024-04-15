using _Game.Scripts.CharacterRelated.Actors.ActorSystems;
using UnityEngine;

namespace _Game.Scripts.CharacterRelated.Actors
{
    [RequireComponent(typeof(ActorHealth))]
    public class DeathWithEffects : MonoBehaviour
    {
        [SerializeField] private GameObject deathEffectPrefab;
        private ActorHealth _actorHealth;

        private void Awake()
        {
            _actorHealth = GetComponent<ActorHealth>();
            _actorHealth.OnDeath += OnDeath;
        }

        private void OnDeath(ActorHealth actorHealth)
        {
            _actorHealth.gameObject.SetActive(false);
            var spawnedDeathEffect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Destroy(spawnedDeathEffect, 1f);
        }
    }
}