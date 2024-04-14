using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Summon.Data;
using _Game.Scripts.Summon.Enums;

namespace _Game.Scripts.Summon
{
    public class Dungeon
    {
        // Probably it should be readonly and inject by zenject
        private DungeonData _dungeonData;
        
        private List<Room> _rooms;
        
        public IReadOnlyList<Room> Rooms => _rooms;

        public void Init(DungeonData dungeonData)
        {
            _dungeonData = dungeonData;
            _rooms = dungeonData.Rooms.Select(it => new Room(it)).ToList();
        }

        public void AddRoom(RoomType roomType)
        {
            _rooms.Add(new Room(roomType, new()));
        }
    }
}