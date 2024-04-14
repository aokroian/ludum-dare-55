using System;
using Common;
using Scene;
using Sounds;
using UI.Dialogue;
using UnityEngine;
using Upgrades;
using Utils;

namespace Game
{
    public class DialogueController : SingletonScene<DialogueController>
    {
        [SerializeField] private DialoguePanel dialoguePanel;

        [Header("Resources")]
        [SerializeField] private Sprite npc1Portrait;
        [SerializeField] private Sprite letterSprite;

        private bool _introShowed;
        public bool IntroShowed => _introShowed;

        private PackageToDeliver _deliveredPackage;
        
        public void Intro(Action action = null)
        {
            if (_introShowed)
            {
                action?.Invoke();
            }

            _introShowed = true;
            
            var config = new DialogueConfig
            {
                portrait = npc1Portrait,
                title = NameGenerator.GetRandomName(),
                message = $"Greetings! Can you please deliver the letter to my cousin?",
                confirmBtnText = "Ok",
                // cancelBtnText = "Fine"
            };
            dialoguePanel.Show(config, () => ReceivePackage(-1, action));
        }

        // private void Intro1(Action action)
        // {
        //     var config = new DialogueConfig
        //     {
        //         portrait = letterSprite,
        //         // title = "NPC_NAME",
        //         message = "You received envelope.",
        //         confirmBtnText = "Ok",
        //         // cancelBtnText = "Fine"
        //     };
        //     dialoguePanel.Show(config, action);
        //     PackageController.Instance.ReceivePackage(-1);
        // }

        public void DeliverPackage(PackageToDeliver package, Action anyAction)
        {
            _deliveredPackage = package;
            var config = new DialogueConfig
            {
                portrait = npc1Portrait,
                title = package.receiverName,
                message = "Thank you! Can you also deliver letter for my cousin? He lives on the next level.",
                confirmBtnText = "Ok",
                cancelBtnText = package.receiverDepth == 0 ? "Later" : null
            };

            dialoguePanel.Show(config, () => UpgradeCallback(anyAction), anyAction);
            PackageController.Instance.DeliverPackage();
            SoundSystem.PlayDeliverySuccessSound();
        }

        public void AlreadyDelivered(Action anyAction)
        {
            var config = new DialogueConfig
            {
                portrait = npc1Portrait,
                title = _deliveredPackage.receiverName,
                message = "Help me to deliver the letter",
                confirmBtnText = "Ok",
                cancelBtnText = _deliveredPackage.receiverDepth == 0 ? "Later" : null
            };

            dialoguePanel.Show(config, () => UpgradeCallback(anyAction), anyAction);
            PackageController.Instance.DeliverPackage();
        }
        
        private void NextMsgCallback(Action action)
        {
            ReceivePackage(_deliveredPackage.receiverDepth, action);
        }

        private void UpgradeCallback(Action action)
        {
            UpgradeSystemUI.Instance.ShowUpgradeSelection(_ => NextMsgCallback(action));
        }

        public void ReceivePackage(int curDepth, Action confirm)
        {
            var config = new DialogueConfig
            {
                portrait = letterSprite,
                // title = "NPC_NAME",
                message = "You received envelope.",
                confirmBtnText = "Ok",
                // cancelBtnText = "Fine"
            };
            dialoguePanel.Show(config, confirm);
            PackageController.Instance.ReceivePackage(curDepth);
            SoundSystem.PlayDeliveryReceivedSound();
        }
        
        public void CantGoUpstairs(Action action = null)
        {
            var config = new DialogueConfig
            {
                portrait = null,
                title = "Can't go upstairs",
                message = "I have a letter to deliver here.",
                confirmBtnText = "Ok",
                cancelBtnText = "Fine"
            };
            dialoguePanel.Show(config, action, action);
        }

        public void PlayerDeath(Action confirm, Action cancel)
        {
            var deliveredCount = PackageController.Instance.deliveredCount;
            var config = new DialogueConfig
            {
                portrait = null,
                title = "You died",
                message = $"You delivered {deliveredCount} packages. Do you want to try again?",
                confirmBtnText = "Restart",
                cancelBtnText = "Main menu"
            };
            dialoguePanel.Show(config, confirm, cancel);
        }

        public void Ending()
        {
            var config = new DialogueConfig
            {
                portrait = null,
                title = "The End",
                message = "You delivered package. Thank you for playing!",
                confirmBtnText = "Main menu"
            };
            dialoguePanel.Show(config, SceneController.Instance.LoadStartMenuScene);
        }
    }
}