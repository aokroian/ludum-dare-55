using System;
using _Game.Scripts.CharacterRelated.Common;
using UnityEngine;

namespace _Game.Scripts.CharacterRelated.Utils
{
    public class CommonUtils
    {
        public static Vector3Int DirectionToVector(WallDirection direction)
        {
            switch (direction)
            {
                case WallDirection.Top:
                    return Vector3Int.up;
                case WallDirection.Right:
                    return Vector3Int.right;
                case WallDirection.Bottom:
                    return Vector3Int.down;
                case WallDirection.Left:
                    return Vector3Int.left;
                default:
                    throw new ArgumentException("Wrong direction");
            }
        }
        
        public static WallDirection VectorToDirection(Vector3Int vector)
        {
            if (vector == Vector3Int.up)
                return WallDirection.Top;
            if (vector == Vector3Int.right)
                return WallDirection.Right;
            if (vector == Vector3Int.down)
                return WallDirection.Bottom;
            if (vector == Vector3Int.left)
                return WallDirection.Left;
            throw new ArgumentException("Vector is not a direction");
        }

        public static WallDirection OppositeDirection(WallDirection direction)
        {
            switch (direction)
            {
                case WallDirection.Top:
                    return WallDirection.Bottom;
                case WallDirection.Right:
                    return WallDirection.Left;
                case WallDirection.Bottom:
                    return WallDirection.Top;
                case WallDirection.Left:
                    return WallDirection.Right;
                default:
                    throw new ArgumentException("Wrong direction");
            }
        }
    }
}