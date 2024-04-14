using System.Collections.Generic;
using _Game.Scripts.Summon.Data;

namespace _Game.Scripts.Summon
{
    public class Dungeon
    {
        // Probably it should be readonly and inject by zenject
        private DungeonData _dungeonData;
        
        private List<Room> _rooms;

        public Dungeon()
        {
        }

        public void Init(DungeonData dungeonData)
        {
            _dungeonData = dungeonData;
        }
    }
}