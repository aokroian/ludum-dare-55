using UnityEngine;

namespace Map
{
    public class LevelConstructionConfig
    {
        public readonly Vector3Int startPos;
        public readonly int roomsCount;
        public readonly BoundsInt bounds;

        public LevelConstructionConfig(Vector3Int startPos, int roomsCount, BoundsInt bounds)
        {
            this.startPos = startPos;
            this.roomsCount = roomsCount;
            this.bounds = bounds;
        }

        public LevelConstructionConfig(Vector3Int startPos, int roomsCount, bool restrictZeroY)
        {
            this.startPos = startPos;
            this.roomsCount = roomsCount;
            this.bounds = restrictZeroY
                ? new BoundsInt(new Vector3Int(-50, 0, -50), new Vector3Int(100, 50, 100))
                : new BoundsInt(new Vector3Int(-50, -50, -50), new Vector3Int(100, 100, 100));
        }
    }
}