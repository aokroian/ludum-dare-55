using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.CharacterRelated.Map.Actors
{
    public class Level : MonoBehaviour
    {
        public Dictionary<Vector3Int, Room> rooms = new();
        
        public Vector3Int GetRoomPosition(Room room)
        {
            foreach (var roomPosition in rooms.Keys)
            {
                if (rooms[roomPosition] == room)
                {
                    return roomPosition;
                }
            }

            throw new ArgumentException("Room not found in level");
        }
    }
}