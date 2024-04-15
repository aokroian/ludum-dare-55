using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace _Game.Scripts.Console
{
    public class PlayerHelpUI : MonoBehaviour, IPointerDownHandler
    {
        [Inject] private GlobalInputSwitcher _globalInputSwitcher;

        [SerializeField] private CanvasGroup playerControlsHelp;
        [SerializeField] private GameObject playerWeaponControlsHelp;
        [SerializeField] private CanvasGroup consoleControlsHelp;

        private bool _arePlayerControlsAllowed;
        private bool _areWeaponControlsAllowed;
        private bool _isConsoleInputOn;

        public void OnPointerDown(PointerEventData eventData)
        {
            _globalInputSwitcher.SwitchToPlayerInput();
        }

        public void OnGlobalInputSwitch(bool isConsoleInputOn)
        {
            _isConsoleInputOn = isConsoleInputOn;
            playerControlsHelp.alpha = isConsoleInputOn ? .5f : 1f;
            consoleControlsHelp.alpha = isConsoleInputOn ? 1f : .5f;

            playerControlsHelp.gameObject.SetActive(_arePlayerControlsAllowed);
            playerWeaponControlsHelp.SetActive(_areWeaponControlsAllowed);
        }

        private void OnGameSummoned()
        {
            _arePlayerControlsAllowed = false;
            _areWeaponControlsAllowed = false;
            OnGlobalInputSwitch(_isConsoleInputOn);
        }

        private void OnPlayerSummoned()
        {
            _arePlayerControlsAllowed = true;
            OnGlobalInputSwitch(_isConsoleInputOn);
        }

        private void OnWeaponSummoned()
        {
            _areWeaponControlsAllowed = true;
            OnGlobalInputSwitch(_isConsoleInputOn);
        }
    }
}