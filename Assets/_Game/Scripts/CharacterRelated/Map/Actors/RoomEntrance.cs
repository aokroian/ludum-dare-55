using Common;
using UnityEngine;

namespace Map.Model
{
    public class RoomEntrance : MonoBehaviour
    {
        [SerializeField] private WallDirection direction;
        public WallDirection Direction => direction;
    }
}