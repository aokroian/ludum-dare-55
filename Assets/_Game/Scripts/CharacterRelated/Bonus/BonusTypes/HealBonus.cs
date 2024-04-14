using Actors;
using Actors.ActorSystems;
using Actors.InputThings;
using UnityEngine;

namespace Bonus.BonusTypes
{
    public class HealBonus : AbstractBonus
    {
        [SerializeField] private int healAmount = 1;
        
        protected override void ApplyBonus(PlayerActorInput player)
        {
            player.GetComponent<ActorHealth>().Heal(healAmount);
            Destroy(gameObject);
        }
    }
}