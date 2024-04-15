using _Game.Scripts.CharacterRelated.Common;
using _Game.Scripts.CharacterRelated.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.CharacterRelated.Game
{
    public class PackageController : SingletonScene<PackageController>
    {
        [SerializeField] private Image packageImage;
        
        public int deliveredCount;
        public PackageToDeliver currentPackage;
        private PackageToDeliver _lastPackage;
        
        public void ReceivePackage(int senderDepth)
        {
            currentPackage = new PackageToDeliver(senderDepth,
                senderDepth + 1,
                _lastPackage?.receiverName ?? "Palych",
                NameGenerator.GetRandomName());
            
            packageImage.gameObject.SetActive(true);
        }
        
        public void DeliverPackage()
        {
            _lastPackage = currentPackage;
            deliveredCount++;
            currentPackage = null;
            
            packageImage.gameObject.SetActive(false);
        }
    }

    public class PackageToDeliver
    {
        public readonly int senderDepth;
        public readonly int receiverDepth;
        public readonly string senderName;
        public readonly string receiverName;
        
        public PackageToDeliver(int senderDepth, int receiverDepth, string senderName, string receiverName)
        {
            this.senderDepth = senderDepth;
            this.receiverDepth = receiverDepth;
            this.senderName = senderName;
            this.receiverName = receiverName;
        }
    }
}