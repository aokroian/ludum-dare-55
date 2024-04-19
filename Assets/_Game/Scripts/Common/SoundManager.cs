using _Game.Scripts.GameLoop.Events;
using _Game.Scripts.Map.Events;
using _Game.Scripts.Story.Events;
using DG.Tweening;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Common
{
    public class SoundManager : MonoBehaviour
    {
        [Header("Console")]
        [SerializeField] private AudioClip[] typingSounds;
        [SerializeField] private AudioClip submitKeySound;
        [SerializeField] private AudioClip universalSummonSound;
        [SerializeField] private AudioClip notificationSound;
        [SerializeField] private AudioSource consoleAudioSource;
        [Header("Characters")]
        [SerializeField] private AudioClip playerDamageSound;
        [SerializeField] private AudioClip playerGunPickupSound;
        [SerializeField] private AudioClip playerCoinPickupSound;
        [SerializeField] private AudioClip playerKeyPickupSound;
        [SerializeField] private AudioClip playerDeathSound;
        [SerializeField] private AudioClip bulletShotSound;
        [SerializeField] private AudioClip bulletHitSound;
        [Space]
        [SerializeField] private AudioClip enemyDeathSound;
        [SerializeField] private AudioClip princessDeathSound;
        [Header("Music")]
        [SerializeField] private AudioClip defaultMusic;
        [SerializeField] private AudioClip storeMusic;
        [SerializeField] private AudioSource musicAudioSource1;
        [SerializeField] private AudioSource musicAudioSource2;
        [Header("GameLoop")]
        [SerializeField] private AudioSource gameEndAudioSource;
        [SerializeField] private AudioClip doorOpenSound;
        [SerializeField] private AudioClip[] happyEndSounds;
        [SerializeField] private AudioClip[] sadEndSounds;
        [Space]
        [SerializeField] private AudioClip switchToConsoleControlsSound;
        [SerializeField] private AudioClip switchToPlayerControlsSound;

        [Inject]
        private SignalBus _signalBus;

        private AudioSource _activeMusicSource;

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

        private float _lastConsoleTypingSoundTime;
        private float _lastNotificationSoundTime;

        private void Awake()
        {
            _signalBus.Subscribe<EndingStartedEvent>(eventData =>
            {
                PlayGameEndingSound(eventData.EndingData.IsGoodEnding);
            });
            _signalBus.Subscribe<PlayerWeaponPickupEvent>(PlayPlayerGunPickupSound);
            _signalBus.Subscribe<GameStartEvent>(PlayDefaultMusic);
            _signalBus.Subscribe<PlayerCoinPickupEvent>(PlayPlayerCoinPickupSound);
            _signalBus.Subscribe<PlayerKeyPickupEvent>(PlayPlayerKeyPickupSound);
            _signalBus.Subscribe<PlayerDoorOpenEvent>(PlayDoorOpenSound);
        }

        public void PlayBardMusic(AudioClip musicClip)
        {
            FadeInMusic(musicClip, 1f);
        }

        public void PlayDefaultMusic()
        {
            FadeInMusic(defaultMusic, 0.6f);
        }

        public void PlayStoreMusic()
        {
            FadeInMusic(storeMusic, 0.5f);
        }

        private void FadeInMusic(AudioClip musicClip, float vol = 1f)
        {
            if (_activeMusicSource != null)
                _activeMusicSource.DOFade(0f, .7f);
            var nextAudioSource = _activeMusicSource == musicAudioSource1 ? musicAudioSource2 : musicAudioSource1;
            if (musicClip != null)
            {
                nextAudioSource.clip = musicClip;
                nextAudioSource.Play();
                nextAudioSource.DOFade(vol, .7f);
            }

            _activeMusicSource = nextAudioSource;
        }

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

        public void PlayNotificationSound()
        {
            if (Time.time - _lastNotificationSoundTime < .2f)
                return;
            _lastNotificationSoundTime = Time.time;
            consoleAudioSource.transform.position = CamPosition;
            consoleAudioSource.PlayOneShot(notificationSound);
        }

        private void PlayGameEndingSound(bool isGoodEnding)
        {
            var rand = Random.Range(0, isGoodEnding ? happyEndSounds.Length : sadEndSounds.Length);
            var clip = isGoodEnding ? happyEndSounds[rand] : sadEndSounds[rand];
            FadeInMusic(null);
            gameEndAudioSource.transform.position = CamPosition;
            gameEndAudioSource.volume = .25f;
            gameEndAudioSource.PlayOneShot(clip);
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

        private void PlayPlayerGunPickupSound()
        {
            consoleAudioSource.transform.position = CamPosition;
            consoleAudioSource.PlayOneShot(playerGunPickupSound);
        }

        private void PlayPlayerCoinPickupSound()
        {
            consoleAudioSource.transform.position = CamPosition;
            consoleAudioSource.PlayOneShot(playerCoinPickupSound);
        }

        private void PlayPlayerKeyPickupSound()
        {
            consoleAudioSource.transform.position = CamPosition;
            consoleAudioSource.PlayOneShot(playerKeyPickupSound);
        }

        private void PlayDoorOpenSound()
        {
            consoleAudioSource.transform.position = CamPosition;
            consoleAudioSource.PlayOneShot(doorOpenSound);
        }

        public void PlayTypeSound()
        {
            if (Time.time - _lastConsoleTypingSoundTime < 0.05f)
                return;
            // slightly randomize pitch to make typing sound more natural
            var pitch = Random.Range(0.93f, 1.07f);
            var rand = Random.Range(0, typingSounds.Length);
            consoleAudioSource.transform.position = CamPosition;
            consoleAudioSource.pitch = pitch;
            consoleAudioSource.PlayOneShot(typingSounds[rand]);
            _lastConsoleTypingSoundTime = Time.time;
        }

        public void PlaySubmitKeySound()
        {
            if (Time.time - _lastConsoleTypingSoundTime < 0.05f)
                return;
            consoleAudioSource.transform.position = CamPosition;
            consoleAudioSource.PlayOneShot(submitKeySound);
            _lastConsoleTypingSoundTime = Time.time;
        }
    }
}