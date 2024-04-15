using System;
using System.Collections;
using _Game.Scripts.CharacterRelated.Common;
using _Game.Scripts.CharacterRelated.Sounds;
using _Game.Scripts.CharacterRelated.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Scripts.CharacterRelated.Scene
{
    public class SceneController : SingletonGlobal<SceneController>
    {
        public const int indexStartMenu = 0;
        public const int indexGame = 1;
        
        public event Action<UnityEngine.SceneManagement.Scene> OnInitialized;

        [SerializeField] private UIElem loadingScreen;

        private int _currentSceneIndex;


        protected override void Awake()
        {
            base.Awake();
            // Don't add anything here, use Start() instead. Object can be destroyed in Awake() if it's a duplicate.
        }

        private bool _TMP_ALREADY_LOADED;

        private void Start()
        {
            Application.targetFrameRate = 60;
            loadingScreen = GetComponentInChildren<UIElem>(true);

            SceneManager.sceneLoaded += (scene, _) => StartCoroutine(InitializeComponentsCoroutine(scene));

            var activeScene = SceneManager.GetActiveScene();
#if UNITY_EDITOR
            // if ((activeScene.buildIndex is indexGame) && !_TMP_ALREADY_LOADED)
            // {
                StartCoroutine(InitializeComponentsCoroutine(activeScene));
            // }

            _TMP_ALREADY_LOADED = true;
#endif
            
            if (activeScene.buildIndex == indexStartMenu)
            {
                SoundSystem.PlayMenuMusic();
            } else if (activeScene.buildIndex == indexGame)
            {
                SoundSystem.PlayCombatMusic();
            }
        }
        
        private IEnumerator InitializeComponentsCoroutine(UnityEngine.SceneManagement.Scene scene)
        {
            yield return null;
            InitializeComponents(scene);
        }

        // Because we need to all Start methods to be called before we initialize components
        private void InitializeComponents(UnityEngine.SceneManagement.Scene scene)
        {
            foreach (var obj in scene.GetRootGameObjects())
            {
                foreach (var component in obj.GetComponentsInChildren<Initializable>())
                {
                    component.Initialize();
                }
            }

            OnInitialized?.Invoke(scene);
            loadingScreen.Hide();
            if (scene.buildIndex != indexGame)
            {
                Cursor.visible = true;
            }
        }

        public void LoadGameScene()
        {
            if (_currentSceneIndex != indexGame)
                SoundSystem.PlayCombatMusic();
            SwitchScene(indexGame);
        }

        public void LoadStartMenuScene()
        {
            if (_currentSceneIndex != indexStartMenu)
                SoundSystem.PlayMenuMusic();
            SwitchScene(indexStartMenu);
            Cursor.visible = true;
        }
        
        private void SwitchScene(int sceneIndex)
        {
            loadingScreen.Show();
            SceneManager.LoadScene(sceneIndex);
            _currentSceneIndex = sceneIndex;
        }
    }
}