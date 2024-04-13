using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioClip[] typingSounds;
        [SerializeField] private AudioClip submitKeySound;
        [SerializeField] private AudioSource consoleAudioSource;

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