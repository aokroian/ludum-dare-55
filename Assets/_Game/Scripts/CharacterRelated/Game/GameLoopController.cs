using System;
using Actors;
using Actors.ActorSystems;
using Actors.InputThings;
using Common;
using DG.Tweening;
using Map.Runtime;
using Scene;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameLoopController : SingletonScene<GameLoopController>, Initializable
    {
        [SerializeField] private GameObject playerPrefab;
        // [SerializeField] private LevelGenerator levelGenerator;
        [SerializeField] private FloorController floorController;

        [SerializeField] private Image foregroundPanel;
        [SerializeField] private TMP_Text floorText;

        public bool skipIntro;
        

        public void Initialize()
        {
            var player = SpawnPlayer();
            floorController.TryToEnterLevel(player, 0, true, Vector3Int.zero);
        }

        private PlayerActorInput SpawnPlayer()
        {
            var playerObj = Instantiate(playerPrefab);
            var playerInput = playerObj.GetComponent<PlayerActorInput>();
            playerObj.GetComponent<ActorHealth>().OnDeath += _ => OnPlayerDeath(playerInput);
            return playerInput;
        }

        private void OnPlayerDeath(PlayerActorInput playerInput)
        {
            playerInput.ToggleInput(false);
            DialogueController.Instance.PlayerDeath(
                () => RestartGame(),
                () => SceneController.Instance.LoadStartMenuScene()
            );
        }
        
        public void RestartGame()
        {
            SceneController.Instance.LoadGameScene();
        }

        public void PauseGame()
        {
            Time.timeScale = 0;
        }

        public void ResumeGame()
        {
            Time.timeScale = 1;
        }

        public void NextFloorAnim(int nextFloor, Action action)
        {
            foregroundPanel.gameObject.SetActive(true);
            floorText.gameObject.SetActive(true);
            foregroundPanel.color = Color.clear;
            floorText.color = Color.clear;
            floorText.text = $"Level {nextFloor + 1}";
            
            var sequence = DOTween.Sequence(transform);
            sequence.Append(foregroundPanel.DOColor(Color.black, 0.6f).OnComplete(() => action?.Invoke()));
            sequence.Append(foregroundPanel.DOColor(Color.clear, 0.6f));
            sequence.Append(floorText.DOColor(Color.white, 0.6f));
            sequence.Append(floorText.DOColor(Color.clear, 0.6f));
            sequence.OnComplete(
                () =>
                {
                    floorText.gameObject.SetActive(false);
                    foregroundPanel.gameObject.SetActive(false);
                });
        }
    }
}