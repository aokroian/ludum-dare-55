using _Game.Scripts.CharacterRelated.Map;
using _Game.Scripts.CharacterRelated.Map.Actors;
using UnityEngine;

namespace _Game.Scripts.CharacterRelated._TMP
{
    public class TmpLevelGenerator : MonoBehaviour
    {
        [SerializeField] private LevelGenerator levelGenerator;
        [SerializeField] private int roomsCount = 10;

        private Level prevLevel;

        public void Generate()
        {
            Destroy(prevLevel?.gameObject);
            var config = new LevelConstructionConfig(Vector3Int.zero, roomsCount, true);
            prevLevel = levelGenerator.GenerateLevel(config);
        }
    }
}