using System.Collections.Generic;
using _Game.Scripts.Summon.Data;
using _Game.Scripts.Summon.Enums;

namespace _Game.Scripts.Summon
{
    public class Room
    {
        private readonly RoomData _roomData;

        public Room(RoomData roomData)
        {
            _roomData = roomData;
            
            _roomData.Entities ??= new();
        }
        
        public Room(RoomType roomType, List<SummonedEntityData> entities)
        {
            _roomData = new RoomData
            {
                RoomType = roomType,
                Entities = entities
            };
        }
    }
}