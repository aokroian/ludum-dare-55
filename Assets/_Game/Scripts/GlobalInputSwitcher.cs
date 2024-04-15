using _Game.Scripts.Console;
using _Game.Scripts.Summon.Data;
using Actors.InputThings;
using UnityEngine;
using Zenject;

namespace _Game.Scripts
{
    public class GlobalInputSwitcher : ITickable
    {
        private SummonedObjectsHolder _objectsHolder;
        private ConsoleView _consoleView;
        private bool _isPlayerInputActive;
        private bool _isLockedToConsole;

        public GlobalInputSwitcher(SummonedObjectsHolder objectsHolder)
        {
            _objectsHolder = objectsHolder;
            SwitchToConsole();
        }

        public void Init(ConsoleView consoleView)
        {
            _consoleView = consoleView;
        }

        public void Tick()
        {
            if (_consoleView == null)
                return;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_isPlayerInputActive)
                    SwitchToConsole();
                else
                    SwitchToPlayerInput();
            }
        }

        public void SwitchToPlayerInput()
        {
            if (_consoleView == null)
                return;
            if (_isLockedToConsole)
                return;
            if (!TogglePlayerInput(true))
                return;
            _consoleView.OnGlobalInputSwitch(false);
            _isPlayerInputActive = true;
        }

        public void ToggleLockToConsole(bool isLocked)
        {
            if (isLocked)
                SwitchToConsole();
            _isLockedToConsole = isLocked;
        }

        public void SwitchToConsole()
        {
            if (_consoleView == null)
                return;
            if (!TogglePlayerInput(false))
                return;
            _consoleView.OnGlobalInputSwitch(true);
            _isPlayerInputActive = false;
        }

        public void EnableAllInput()
        {
            _consoleView.EnableInput();
            if (_isPlayerInputActive)
                TogglePlayerInput(true);
        }

        public void DisableAllInput()
        {
            _consoleView.DisableInput();
            TogglePlayerInput(false);
        }

        private bool TogglePlayerInput(bool isOn)
        {
            var player = _objectsHolder.GetPlayer();
            if (player == null)
                return false;

            player.gameObject.GetComponent<PlayerActorInput>().ToggleInput(isOn);
            return true;
        }
    }
}