using _Game.Scripts.GameLoop.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace _Game.Scripts.Console
{
    public class ControlsHelpUI : MonoBehaviour, IPointerDownHandler
    {
        [Inject] private GlobalInputSwitcher _globalInputSwitcher;

        [SerializeField] private CanvasGroup playerControlsHelp;
        [SerializeField] private GameObject playerWeaponControlsHelp;
        [SerializeField] private GameObject playerSwitchControlsHelp;
        [SerializeField] private CanvasGroup consoleControlsHelp;
        [SerializeField] private GameObject consoleSwitchControlsHelp;

        private bool _areConsoleControlsAllowed;
        private bool _arePlayerControlsAllowed;
        private bool _areWeaponControlsAllowed;
        private bool _isConsoleInputOn;


        [Inject]
        public void Construct(SignalBus signalBus)
        {
            signalBus.Subscribe<GameStartEvent>(OnGameSummoned);
            signalBus.Subscribe<SummonPlayerEvent>(OnPlayerSummoned);
            signalBus.Subscribe<SummonPlayerWeaponEvent>(OnWeaponSummoned);
        }

        private void Start()
        {
            OnGlobalInputSwitch(true);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _globalInputSwitcher.SwitchToPlayerControls();
        }


        public void OnGlobalInputSwitch(bool isConsoleInputOn)
        {
            _isConsoleInputOn = isConsoleInputOn;
            playerControlsHelp.alpha = isConsoleInputOn ? .5f : 1f;
            consoleControlsHelp.alpha = isConsoleInputOn ? 1f : .5f;

            playerControlsHelp.gameObject.SetActive(_arePlayerControlsAllowed);
            playerWeaponControlsHelp.SetActive(_areWeaponControlsAllowed);
            consoleControlsHelp.gameObject.SetActive(_areConsoleControlsAllowed);

            consoleSwitchControlsHelp.SetActive(_arePlayerControlsAllowed && isConsoleInputOn);
            playerSwitchControlsHelp.SetActive(!isConsoleInputOn);
        }

        private void OnGameSummoned()
        {
            _areConsoleControlsAllowed = true;
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