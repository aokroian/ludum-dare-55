using _Game.Scripts.CharacterRelated.Common;
using UnityEngine;

namespace _Game.Scripts.CharacterRelated.Map.Actors
{
    public class RoomEntrance : MonoBehaviour
    {
        [SerializeField] private WallDirection direction;
        public WallDirection Direction => direction;
    }
}