using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Common
{
    public class SoundManager : MonoBehaviour
    {
        [Header("Console")]
        [SerializeField] private AudioClip[] typingSounds;
        [SerializeField] private AudioClip submitKeySound;
        [SerializeField] private AudioClip universalSummonSound;
        [SerializeField] private AudioSource consoleAudioSource;
        [Header("Characters")]
        [SerializeField] private AudioClip playerDamageSound;
        [SerializeField] private AudioClip playerDeathSound;
        [SerializeField] private AudioClip bulletShotSound;
        [SerializeField] private AudioClip bulletHitSound;
        [Space]
        [SerializeField] private AudioClip enemyDeathSound;
        [SerializeField] private AudioClip princessDeathSound;
        [Header("Music")]
        [SerializeField] private AudioClip defaultMusic;
        [SerializeField] private AudioClip storeMusic;
        [Header("GameLoop")]
        [SerializeField] private AudioClip[] happyEndSounds;
        [SerializeField] private AudioClip[] sadEndSounds;
        [Space]
        [SerializeField] private AudioClip switchToConsoleControlsSound;
        [SerializeField] private AudioClip switchToPlayerControlsSound;


        private Camera _mainCam;
        private Vector3 CamPosition
        {
            get
            {
                if (_mainCam == null)
                    _mainCam = Camera.main;
                return _mainCam != null ? _mainCam.transform.position : Vector3.zero;
            }
        }

        private float _lastConsoleSoundTime;

        public void PlayControlsSwitchSound(bool isToConsoleControls)
        {
            consoleAudioSource.transform.position = CamPosition;
            consoleAudioSource.PlayOneShot(isToConsoleControls
                ? switchToConsoleControlsSound
                : switchToPlayerControlsSound);
        }

        public void PlayUniversalSummonSound()
        {
            consoleAudioSource.transform.position = CamPosition;
            consoleAudioSource.PlayOneShot(universalSummonSound);
        }

        public void PlayGameEndingSound(bool isPositive)
        {
            var rand = Random.Range(0, isPositive ? happyEndSounds.Length : sadEndSounds.Length);
            consoleAudioSource.transform.position = CamPosition;
            consoleAudioSource.PlayOneShot(isPositive ? happyEndSounds[rand] : sadEndSounds[rand]);
        }

        public void PlayBulletShotSound(Vector3 position)
        {
            AudioSource.PlayClipAtPoint(bulletShotSound, position);
        }

        public void PlayBulletHitSound(Vector3 position)
        {
            AudioSource.PlayClipAtPoint(bulletHitSound, position);
        }

        public void PlayPlayerDamageSound(AudioSource playerAudioSource)
        {
            playerAudioSource.PlayOneShot(playerDamageSound);
        }

        public void PlayPlayerDeathSound(AudioSource playerAudioSource)
        {
            playerAudioSource.PlayOneShot(playerDeathSound);
        }

        public void PlayTypeSound()
        {
            if (Time.time - _lastConsoleSoundTime < 0.05f)
                return;
            // slightly randomize pitch to make typing sound more natural
            var pitch = Random.Range(0.93f, 1.07f);
            var rand = Random.Range(0, typingSounds.Length);
            consoleAudioSource.transform.position = CamPosition;
            consoleAudioSource.pitch = pitch;
            consoleAudioSource.PlayOneShot(typingSounds[rand]);
            _lastConsoleSoundTime = Time.time;
        }

        public void PlaySubmitKeySound()
        {
            if (Time.time - _lastConsoleSoundTime < 0.05f)
                return;
            consoleAudioSource.transform.position = CamPosition;
            consoleAudioSource.PlayOneShot(submitKeySound);
            _lastConsoleSoundTime = Time.time;
        }
    }
}