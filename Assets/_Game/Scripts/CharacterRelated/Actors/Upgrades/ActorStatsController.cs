using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.CharacterRelated.Actors.Upgrades
{
    public class ActorStatsController : MonoBehaviour
    {
        [field: SerializeField] public ActorStatsSo Stats { get; private set; }
        private readonly List<IActorStatsReceiver> _statsReceivers = new();

        private void Awake()
        {
            var copy = Stats.GetCopy();
            Stats = copy;
            Stats.OnValidateEvent += ApplyStatsToAllReceivers;
        }

        public void ModifyCurrentStats(ActorStatsSo actorStatsToAdd)
        {
            if (actorStatsToAdd.description == "Random")
            {
                AddRandomStats();
                return;
            }

            Stats.addedMovementSpeed += actorStatsToAdd.addedMovementSpeed;
            Stats.addedScaleModifier += actorStatsToAdd.addedScaleModifier;
            Stats.addedMaxHealth += actorStatsToAdd.addedMaxHealth;
            Stats.addedShootRate += actorStatsToAdd.addedShootRate;
            Stats.addedBulletsSpeed += actorStatsToAdd.addedBulletsSpeed;
            Stats.addedBulletsDamage += actorStatsToAdd.addedBulletsDamage;
            Stats.addedBulletsScale += actorStatsToAdd.addedBulletsScale;
            Stats.addedBulletsPerShotCount += actorStatsToAdd.addedBulletsPerShotCount;
            Stats.addedBulletsPiercingCount += actorStatsToAdd.addedBulletsPiercingCount;
            ApplyStatsToAllReceivers();
        }

        private void AddRandomStats()
        {
            Stats.addedMovementSpeed += Random.Range(-0.5f, 0.5f);
            Stats.addedScaleModifier += Random.Range(-0.5f, 0.5f);
            Stats.addedMaxHealth += Random.Range(-1, 1);
            Stats.addedShootRate += Random.Range(-0.5f, 0.5f);
            Stats.addedBulletsSpeed += Random.Range(-0.5f, 0.5f);
            Stats.addedBulletsDamage += Random.Range(-1, 1);
            Stats.addedBulletsScale += Random.Range(-0.5f, 0.5f);
            Stats.addedBulletsPerShotCount += Random.Range(-1, 1);
            Stats.addedBulletsPiercingCount += Random.Range(-1, 1);

            ApplyStatsToAllReceivers();
        }

        private void ApplyStatsToAllReceivers()
        {
            foreach (var receiver in _statsReceivers)
                receiver.ReceiveActorStats(Stats);
        }

        public void AddReceiver(IActorStatsReceiver actorStatsReceiver)
        {
            if (_statsReceivers.Contains(actorStatsReceiver))
                return;
            _statsReceivers.Add(actorStatsReceiver);
            actorStatsReceiver.ReceiveActorStats(Stats);
        }

        public void RemoveReceiver(IActorStatsReceiver actorStatsReceiver)
        {
            if (_statsReceivers.Contains(actorStatsReceiver))
                _statsReceivers.Remove(actorStatsReceiver);
        }
    }
}