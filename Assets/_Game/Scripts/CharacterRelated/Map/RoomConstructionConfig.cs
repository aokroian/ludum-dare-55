using _Game.Scripts.CharacterRelated.Common;
using UnityEngine;

namespace _Game.Scripts.CharacterRelated.Map
{
    public struct RoomConstructionConfig
    {
        public readonly WallDirection[] doors;
        public readonly Vector3 position;
        public readonly Transform parent;
        public readonly RoomType roomType;
        public readonly int distanceFromStart;
        
        public RoomConstructionConfig(WallDirection[] doors, Vector3 position, Transform parent, RoomType roomType, int distanceFromStart)
        {
            this.doors = doors;
            this.position = position;
            this.parent = parent;
            this.roomType = roomType;
            this.distanceFromStart = distanceFromStart;
        }
    }
}