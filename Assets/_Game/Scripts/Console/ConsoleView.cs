using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Game.Scripts.Console
{
    public class ConsoleView : MonoBehaviour
    {
        [SerializeField] private bool skipInitAnimation;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private ScrollRect outputScrollRect;
        [SerializeField] private Transform outputContainer;
        [SerializeField] private ConsoleOutputEntry outputEntryPrefab;
        [SerializeField] private int maxOutputEntries = 100;

        private readonly List<ConsoleCommand> _commandsHistory = new();
        [Inject] private ConsoleCommandsHandler _commandsHandler;
        [Inject] private SoundManager _soundManager;

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            _commandsHistory.Clear();
            inputField.onSubmit.RemoveAllListeners();
            inputField.onSubmit.AddListener(OnCommandSubmit);
            inputField.onEndEdit.RemoveAllListeners();
            inputField.onEndEdit.AddListener(FormatInputField);
            inputField.onValueChanged.RemoveAllListeners();
            inputField.onValueChanged.AddListener(OnInputValueChanged);

            outputScrollRect.verticalNormalizedPosition = 0;
            foreach (Transform c in outputContainer)
            {
                Destroy(c.gameObject);
            }

            if (skipInitAnimation)
            {
                inputField.interactable = true;
                inputField.text = "summon game";
                OnCommandSubmit(inputField.text);
            }
            else
            {
                StartCoroutine(GameInitAnimation());
            }
        }

        private void OnInputValueChanged(string currentInputValue)
        {
            outputScrollRect.verticalNormalizedPosition = 0;
        }

        private void OnCommandSubmit(string text)
        {
            inputField.interactable = false;
            _soundManager.PlaySubmitKeySound();
            _commandsHandler.SubmitCommand(text, OnCommandProcessed);

            inputField.caretPosition = inputField.text.Length;
            inputField.ActivateInputField();
            inputField.Select();
        }

        private void OnCommandProcessed(ConsoleOutputData data)
        {
            SpawnOutputEntry(data);
            inputField.text = "";
            inputField.interactable = true;
        }

        private void SpawnOutputEntry(ConsoleOutputData data)
        {
            var spawnedOutputEntry = Instantiate(outputEntryPrefab, outputContainer);
            ((RectTransform)spawnedOutputEntry.transform).pivot = new Vector2(0, 1);
            spawnedOutputEntry.Init(data);
            if (outputContainer.childCount > maxOutputEntries)
                Destroy(outputContainer.GetChild(0).gameObject);
            outputScrollRect.verticalNormalizedPosition = 0;
        }

        private void FormatInputField(string inputText)
        {
            // to lower case
            inputField.text = inputText.ToLower();
        }

        private void Update()
        {
            if (_forcedInputText != null)
            {
                inputField.text = _forcedInputText;
                inputField.caretPosition = inputField.text.Length;
                inputField.ActivateInputField();
                inputField.Select();
            }
        }

        private string _forcedInputText;

        private IEnumerator GameInitAnimation()
        {
            const string initMessage = "summon game";
            var delays = new[] { .25f, .25f, .1f, .35f, .3f, .7f, .15f, .25f, .28f, .2f, .2f };
            inputField.ActivateInputField();
            inputField.Select();
            _forcedInputText = "";
            yield return new WaitForSeconds(2.5f);
            for (var i = 0; i < initMessage.Length; i++)
            {
                _soundManager.PlayTypeSound();
                _forcedInputText = initMessage.Take(i + 1).Aggregate("", (current, c) => current + c);
                yield return new WaitForSeconds(delays[i]);
            }

            yield return new WaitForSeconds(2.5f);
            _forcedInputText = null;
            OnCommandSubmit(initMessage);
        }
    }
}