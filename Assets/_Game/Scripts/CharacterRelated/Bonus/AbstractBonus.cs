using System;
using Actors.InputThings;
using DG.Tweening;
using Sounds;
using UnityEngine;

namespace Bonus
{
    public abstract class AbstractBonus : MonoBehaviour
    {
        protected abstract void ApplyBonus(PlayerActorInput player);


        private void OnDestroy()
        {
            transform.DOKill(false);
        }

        public void SetAndStart(Vector3 position)
        {
            transform.position = position;

            var animPosition = position + new Vector3(0, 0.5f, 0);
            
            var sequence = DOTween.Sequence(transform);
            sequence.Append(transform.DOMove(animPosition, 1f).SetEase(Ease.InOutQuad));
            sequence.Append(transform.DOMove(position, 1f).SetEase(Ease.InOutQuad));
            sequence.SetLoops(-1);
        }
        
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                SoundSystem.PlayCollectableSound(this);
                ApplyBonus(other.GetComponent<PlayerActorInput>());
                gameObject.SetActive(false);
                // Destroy(gameObject);
            }
        }
    }
}