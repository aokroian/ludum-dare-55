using System;
using Scene;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus
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