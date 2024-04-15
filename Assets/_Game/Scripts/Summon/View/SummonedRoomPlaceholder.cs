using System;
using System.Collections.Generic;
using _Game.Scripts.Story;
using UnityEngine;

namespace _Game.Scripts.Summon.View
{
    public class SummonedRoomPlaceholder: SummonedRoom
    {
        public void InitPlaceholder(List<SummonedObject> objects)
        {
            _objects = objects;
        }

        public override IGameplayEvent GetEventIfAny()
        {
            return null;
        }

        public override void AddObject(SummonedObject summonedObject)
        {
            _objects.Add(summonedObject);
            // We don't summon OnMovedToRoom for placeholder
        }
    }
}