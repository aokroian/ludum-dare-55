using _Game.Scripts.Console;
using _Game.Scripts.Summon.Data;
using Actors.InputThings;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using Zenject;

namespace _Game.Scripts
{
    public class GlobalInputSwitcher : ITickable
    {
        private SummonedObjectsHolder _objectsHolder;
        private ConsoleView _consoleView;
        private ControlsHelpUI _controlsHelpUI;
        private bool _isPlayerInputActive;
        private bool _isLockedToConsole;

        public GlobalInputSwitcher(SummonedObjectsHolder objectsHolder)
        {
            _objectsHolder = objectsHolder;
            SwitchToConsoleControls();
        }

        public void Init(ConsoleView consoleView, ControlsHelpUI controlsHelpUI)
        {
            _controlsHelpUI = controlsHelpUI;
            _consoleView = consoleView;
        }

        public void Tick()
        {
            if (_consoleView == null)
                return;
            var isSwitchingInput = false;
            if (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt))
            {
                if (_isPlayerInputActive)
                    SwitchToConsoleControls();
                else
                    SwitchToPlayerControls();
            }
        }

        public void SwitchToPlayerControls()
        {
            var notAllowed = _consoleView == null || _isLockedToConsole || !TogglePlayerInput(true);
            if (notAllowed)
            {
                _consoleView.Invoke(nameof(ConsoleView.SimulatePointerDown), .2f);
                return;
            }

            _consoleView.OnGlobalInputSwitch(false);
            _controlsHelpUI.OnGlobalInputSwitch(false);
            _isPlayerInputActive = true;
        }

        public void ToggleLockToConsole(bool isLocked)
        {
            if (isLocked)
                SwitchToConsoleControls();
            _isLockedToConsole = isLocked;
        }

        public void SwitchToConsoleControls()
        {
            if (_consoleView == null)
                return;
            TogglePlayerInput(false);
            _consoleView.OnGlobalInputSwitch(true);
            _controlsHelpUI.OnGlobalInputSwitch(true);
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