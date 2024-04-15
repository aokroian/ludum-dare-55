using _Game.Scripts.Common;
using _Game.Scripts.GameLoop.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace _Game.Scripts.Console
{
    public class ControlsHelpUI : MonoBehaviour
    {
        [Inject] private GlobalInputSwitcher _globalInputSwitcher;

        [SerializeField] private CanvasGroup playerControlsHelp;
        [SerializeField] private GameObject playerWeaponControlsHelp;
        [SerializeField] private GameObject playerSwitchControlsHelp;
        [SerializeField] private CanvasGroup consoleControlsHelp;
        [SerializeField] private GameObject consoleSwitchControlsHelp;
        [SerializeField] private PointerDownHandler[] pointerDownHandlers;

        private bool _areConsoleControlsAllowed;
        private bool _arePlayerControlsAllowed;
        private bool _areWeaponControlsAllowed;
        private bool _isConsoleInputOn;


        [Inject]
        public void Construct(SignalBus signalBus)
        {
            signalBus.Subscribe<GameStartEvent>(OnGameSummoned);
            signalBus.Subscribe<SummonPlayerEvent>(OnPlayerSummoned);
            signalBus.Subscribe<PlayerWeaponPickupEvent>(OnWeaponSummoned);
        }

        private void Awake()
        {
            foreach (var pointerDownHandler in pointerDownHandlers)
                pointerDownHandler.OnPointerDownEvent += OnPointerDown;
        }

        private void OnDestroy()
        {
            foreach (var pointerDownHandler in pointerDownHandlers)
                pointerDownHandler.OnPointerDownEvent -= OnPointerDown;
        }

        private void Start()
        {
            OnGlobalInputSwitch(true);
        }

        private void OnPointerDown()
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

            foreach (var pointerDownHandler in pointerDownHandlers)
                pointerDownHandler.gameObject.SetActive(isConsoleInputOn);
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