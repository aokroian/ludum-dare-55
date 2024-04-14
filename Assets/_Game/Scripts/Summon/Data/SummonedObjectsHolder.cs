using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Summon.View;
using Unity.VisualScripting;
using UnityEngine;

namespace _Game.Scripts.Summon.Data
{
    public class SummonedObjectsHolder
    {
        public List<SummonedRoom> rooms = new();

        public List<SummonedObject> objectsOutOfRoom = new();

        private SummonedPlayer _summonedPlayer;

        public void AddRoom(SummonedRoom room)
        {
            rooms.Add(room);
        }

        public SummonedRoom GetCurrentPlayerRoom()
        {
            foreach (var room in rooms)
            {
                if (room.Objects.FirstOrDefault(it => it == _summonedPlayer) != null)
                    return room;
            }

            return null;
        }

        public void SetPlayerRef(SummonedPlayer player)
        {
            _summonedPlayer = player;
        }

        public SummonedPlayer GetPlayer()
        {
            return _summonedPlayer;
        }

        public void ClearEverything()
        {
            Object.Destroy(_summonedPlayer);
            _summonedPlayer = null;
            
            foreach (var obj in objectsOutOfRoom)
            {
                Object.Destroy(obj.gameObject);
            }
            objectsOutOfRoom.Clear();
            
            foreach (var room in rooms)
            {
                foreach (var obj in room.Objects)
                {
                    Object.Destroy(obj.gameObject);
                }
                Object.Destroy(room.gameObject);
            }
            rooms.Clear();
        }
    }
}