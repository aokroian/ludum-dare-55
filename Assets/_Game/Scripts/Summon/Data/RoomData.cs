using System;
using System.Collections.Generic;
using _Game.Scripts.Summon.Enums;

namespace _Game.Scripts.Summon.Data
{
    [Serializable]
    public class RoomData
    {
        public RoomType RoomTypeId;
        public List<SummonedEntityData> Entities;
    }
}