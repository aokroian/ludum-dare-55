using _Game.Scripts.CharacterRelated.Actors.ActorSystems;
using _Game.Scripts.CharacterRelated.Actors.InputThings;
using UnityEngine;

namespace _Game.Scripts.CharacterRelated.Bonus.BonusTypes
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