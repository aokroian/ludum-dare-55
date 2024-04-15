using _Game.Scripts.CharacterRelated.Scene;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.CharacterRelated.UI.Menus
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button startBtn;
        [SerializeField] private Button exitBtn;

        private void OnEnable()
        {
            Cursor.visible = true;
        }

        private void OnDisable()
        {
            Cursor.visible = false;
        }

        private void Awake()
        {
            startBtn.onClick.AddListener(StartGame);
            exitBtn.onClick.AddListener(ExitGame);
        }
        
        private void StartGame()
        {
            SceneController.Instance.LoadGameScene();
        }
        
        private void ExitGame()
        {
            Application.Quit();
        }
    }
}