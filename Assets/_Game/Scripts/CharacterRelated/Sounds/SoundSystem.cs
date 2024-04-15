using System.Collections;
using _Game.Scripts.CharacterRelated.Actors.InputThings;
using _Game.Scripts.CharacterRelated.Map.Actors;
using UnityEngine;

namespace _Game.Scripts.CharacterRelated.Sounds
{
    public class SoundSystem
    {
        private static SoundsConfig Sounds
        {
            get
            {
                if (_sounds == null)
                    _sounds = Resources.Load<SoundsConfig>("SoundsConfig");

                return _sounds;
            }
        }

        private static SoundsConfig _sounds;

        private static Camera MainCamera
        {
            get
            {
                if (_camera == null)
                    _camera = Camera.main;
                return _camera;
            }
        }

        private static Camera _camera;


        #region Music

        private static AudioSource MusicAudioSource1
        {
            get
            {
                if (_musicAudioSource1 != null)
                    return _musicAudioSource1;
                var spawnedGo = new GameObject("Music_AudioSource_1");
                _musicAudioSource1 = spawnedGo.AddComponent<AudioSource>();
                _musicAudioSource1.loop = true;
                Object.DontDestroyOnLoad(spawnedGo);

                _crossFadeAudioCoroutineOwner = spawnedGo.AddComponent<DummyMonoBehaviour>();
                return _musicAudioSource1;
            }
        }

        private static AudioSource _musicAudioSource1;

        private static AudioSource MusicAudioSource2
        {
            get
            {
                if (_musicAudioSource2 != null)
                    return _musicAudioSource2;
                var spawnedGo = new GameObject("Music_AudioSource_2");
                _musicAudioSource2 = spawnedGo.AddComponent<AudioSource>();
                _musicAudioSource2.loop = true;
                Object.DontDestroyOnLoad(spawnedGo);

                return _musicAudioSource2;
            }
        }

        private static AudioSource _currentMusicAudioSource;
        private static AudioSource _musicAudioSource2;
        private static Coroutine _crossFadeAudioCoroutine;
        private static MonoBehaviour _crossFadeAudioCoroutineOwner;

        private static IEnumerator CrossFadeAudio(AudioClip clip, float toVolume)
        {
            if (_currentMusicAudioSource != null &&
                _currentMusicAudioSource.isPlaying &&
                _currentMusicAudioSource.clip != null &&
                _currentMusicAudioSource.clip == clip)
            {
                yield break;
            }

            var from = _currentMusicAudioSource == MusicAudioSource2
                ? MusicAudioSource2
                : MusicAudioSource1;
            var to = _currentMusicAudioSource == MusicAudioSource2
                ? MusicAudioSource1
                : MusicAudioSource2;

            _currentMusicAudioSource = to;

            to.clip = clip;
            to.volume = 0f;
            to.Play();

            float t = 0;
            var v = from.volume;

            while (t < 0.98f)
            {
                t = Mathf.Lerp(t, 1f, Time.deltaTime * 1f);
                from.volume = Mathf.Lerp(v, 0f, t);
                to.volume = Mathf.Lerp(0f, toVolume, t);
                yield return null;
            }

            from.Play();
            from.volume = 0f;
            to.volume = toVolume;
        }

        private static void CrossFadeMusic(AudioClip musicClip, float toVolume)
        {
            if (_crossFadeAudioCoroutineOwner == null)
                Debug.Log(MusicAudioSource1.name);
            if (_crossFadeAudioCoroutine != null)
                _crossFadeAudioCoroutineOwner.StopCoroutine(_crossFadeAudioCoroutine);
            _crossFadeAudioCoroutineOwner.StartCoroutine(CrossFadeAudio(musicClip, toVolume));
        }

        public static void PlayMenuMusic()
        {
            CrossFadeMusic(Sounds.menuMusic, .25f);
        }

        public static void PlayCombatMusic()
        {
            CrossFadeMusic(Sounds.combatMusic, .1f);
        }

        public static void PlayPeacefulMusic()
        {
            CrossFadeMusic(Sounds.peacefulMusic, 1f);
        }

        #endregion

        #region Map_Related_Sounds

        public static void PlayMoveToAnotherDepthSound(PlayerActorInput player)
        {
            PlayAtPointIfNotNull(Sounds.moveToAnotherDepthSound, player.transform.position);
        }

        public static void PlayDoorOpenSound(Room room)
        {
            PlayAtPointIfNotNull(Sounds.doorsOpenSound, room.transform.position);
        }

        public static void PlayDoorCloseSound(Room room)
        {
            PlayAtPointIfNotNull(Sounds.doorsCloseSound, room.transform.position);
        }

        public static void PlayCollectableSound(MonoBehaviour collectable)
        {
            PlayAtPointIfNotNull(Sounds.collectableSound, collectable.transform.position);
        }

        public static void PlayDeliveryReceivedSound()
        {
            PlayAtPointIfNotNull(Sounds.deliveryReceivedSound, MainCamera.transform.position);
        }

        public static void PlayDeliverySuccessSound()
        {
            PlayAtPointIfNotNull(Sounds.deliverySuccessSound, MainCamera.transform.position);
        }

        #endregion

        #region UI_Sounds

        public static void PlayMenuButtonClickSound()
        {
            PlayAtPointIfNotNull(Sounds.menuButtonClickSound, MainCamera.transform.position);
        }

        public static void PlayUpgradeButtonClickSound()
        {
            PlayAtPointIfNotNull(Sounds.upgradeButtonClickSound, MainCamera.transform.position);
        }

        public static void PlayDialogueSound()
        {
            PlayAtPointIfNotNull(Sounds.dialogueSound, MainCamera.transform.position);
        }

        #endregion

        private static void PlayAtPointIfNotNull(AudioClip clip, Vector3 position)
        {
            if (clip != null)
                AudioSource.PlayClipAtPoint(clip, position);
        }
    }
}