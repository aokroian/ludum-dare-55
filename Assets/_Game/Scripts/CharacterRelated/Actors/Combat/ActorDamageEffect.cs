using _Game.Scripts.CharacterRelated.Actors.ActorSystems;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.CharacterRelated.Actors.Combat
{
    public class ActorDamageEffect : MonoBehaviour
    {
        private void Awake()
        {
            var actorHealth = GetComponent<ActorHealth>();

            actorHealth.OnDamageTaken += OnDamageTaken;
        }


        private Tweener _tweener;

        private void OnDamageTaken(int damage)
        {
            _tweener?.Kill();
            _tweener = transform.DOScale(.8f, .15f);
            _tweener.onComplete += () => { transform.DOScale(1f, .1f); };
        }
    }
}