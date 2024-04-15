using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Summon.View;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Summon.Data
{
    public class SummonedObjectsHolder
    {
        public IReadOnlyList<SummonedRoom> Rooms => _rooms.Count > 0 ? _rooms : _placeholderRooms;
        public readonly List<SummonedObject> ObjectsOutOfRoom = new();
        public int RealRoomCount => _rooms.Count;

        private SummonedPlayer _summonedPlayer;
        
        private List<SummonedRoom> _placeholderRooms;

        private readonly List<SummonedRoom> _rooms = new();
        
        public void Init(SummonedRoomPlaceholder roomPlaceholder)
        {
            roomPlaceholder.InitPlaceholder(ObjectsOutOfRoom);
            _placeholderRooms = new List<SummonedRoom> {roomPlaceholder};
        }

        public void AddRoom(SummonedRoom room)
        {
            if (_rooms.Count == 0)
            {
                foreach (var obj in ObjectsOutOfRoom)
                {
                    room.AddObject(obj);
                }
                ObjectsOutOfRoom.Clear();
            }
                
            _rooms.Add(room);
        }

        public SummonedRoom GetPlayerRoomOrFirst()
        {
            foreach (var room in _rooms)
            {
                if (room.Objects.FirstOrDefault(it => it == _summonedPlayer) != null)
                    return room;
            }
            if (Rooms.Count > 0)
                return Rooms[0];

            return null;
        }
        
        public int GetPlayerRoomIndex()
        {
            for (int i = 0; i < _rooms.Count; i++)
            {
                if (_rooms[i].Objects.FirstOrDefault(it => it == _summonedPlayer) != null)
                    return i;
            }

            return -1;
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
            
            foreach (var obj in ObjectsOutOfRoom)
            {
                Object.Destroy(obj.gameObject);
            }
            ObjectsOutOfRoom.Clear();
            
            foreach (var room in _rooms)
            {
                foreach (var obj in room.Objects)
                {
                    Object.Destroy(obj.gameObject);
                }
                Object.Destroy(room.gameObject);
            }
            _rooms.Clear();
        }
    }
}