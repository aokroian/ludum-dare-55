using Common;
using UnityEngine;

namespace Bonus
{
    public class BonusSystem : SingletonScene<BonusSystem>
    {
        [SerializeField] private float bonusChance = 0.2f;
        [SerializeField] private AbstractBonus[] bonuses;
        
        public void SpawnRandomBonus(Vector3 position)
        {
            if (Random.value > bonusChance)
                return;
            
            var bonus = Instantiate(bonuses[Random.Range(0, bonuses.Length)]);
            bonus.SetAndStart(position);
        }
    }
}