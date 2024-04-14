using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Summon.View;

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
                if (room.Objects.FirstOrDefault(it => it is SummonedPlayer) != null)
                {
                    return room;
                }
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
    }
}