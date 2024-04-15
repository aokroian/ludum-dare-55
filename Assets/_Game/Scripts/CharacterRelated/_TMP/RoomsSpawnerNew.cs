using System.Collections.Generic;
using _Game.Scripts.CharacterRelated.Common;
using _Game.Scripts.CharacterRelated.Map;
using UnityEngine;

namespace _Game.Scripts.CharacterRelated._TMP
{
    public class RoomsSpawnerNew : MonoBehaviour
    {
        [SerializeField] private Grid globalGrid;
        [SerializeField] private Grid roomGrid;
        [SerializeField] private RoomFactory roomFactory;

        private void Start()
        {
            SpawnRooms();
        }

        private void SpawnRooms()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    var roomPos = globalGrid.CellToWorld(new Vector3Int(x, y, 0));
                    var room = roomFactory.CreateRoom(
                        new RoomConstructionConfig(
                            TMPRandomDirections(),
                            roomPos,
                            roomGrid.transform,
                            RoomType.Common,
                            0
                        ));
                }
            }
        }

        private WallDirection[] TMPRandomDirections()
        {
            var directions = new List<WallDirection>();

            for (int i = 0; i < 4; i++)
            {
                if (Random.Range(0, 2) == 0)
                    directions.Add((WallDirection) i);
            }
            return directions.ToArray();
        }
    }
}