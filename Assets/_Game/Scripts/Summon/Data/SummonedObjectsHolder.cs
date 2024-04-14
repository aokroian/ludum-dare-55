using System.Collections.Generic;
using _Game.Scripts.Summon.View;

namespace _Game.Scripts.Summon.Data
{
    public class SummonedObjectsHolder
    {
        public List<SummonedRoom> rooms = new();

        public List<SummonedObject> objectsOutOfRoom = new();
        
        public void AddRoom(SummonedRoom room)
        {
            rooms.Add(room);
        }
    }
}