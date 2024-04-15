using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.CharacterRelated.Common;
using _Game.Scripts.CharacterRelated.Map.Actors;
using UnityEngine;

namespace _Game.Scripts.CharacterRelated.Map
{
    public class LevelGenerator : MonoBehaviour
    {
        // [SerializeField] private bool randomSeed;
        // [SerializeField] private int seed = 12345;
        [SerializeField] private int retriesMaxCount = 5;
        
        [Space]
        [SerializeField] private Grid globalGrid;
        [SerializeField] private Grid roomGrid;
        [SerializeField] private RoomFactory roomFactory;

        private Dictionary<Vector3Int, RoomSpawnData> roomPositions = new();

        public Level GenerateLevel(LevelConstructionConfig config)
        {
            ClearData();
            
            // if (randomSeed)
            //     seed = Random.Range(0, 1000000);
            //
            // Random.InitState(seed);

            CreateRoomsData(config);
            SetRoomTypes();
            
            var levelObj = new GameObject("Level");
            levelObj.transform.SetParent(roomGrid.transform);
            levelObj.transform.position = Vector3.zero;
            var spawnedRooms = SpawnRooms(levelObj.transform);

            var level = levelObj.AddComponent<Level>();
            level.rooms = spawnedRooms;

            return level;
        }

        private void ClearData()
        {
            roomPositions.Clear();
        }

        private void CreateRoomsData(LevelConstructionConfig config)
        {
            var rooms = new List<RoomSpawnData>();
            var startRoom = new RoomSpawnData(0, config.startPos, RoomType.Start);
            rooms.Add(startRoom);
            roomPositions[config.startPos] = startRoom;
            var roomsPlanned = 1;

            var emergencyExit = 0; // just in case of infinite loop
            var curIndex = 0;
            while (roomsPlanned < config.roomsCount && emergencyExit < 1000)
            {
                for (int retry = 0; retry < retriesMaxCount; retry++)
                {
                    var curRoom = rooms[curIndex % rooms.Count];
                    var newRoomPos = GetRandomNeighborPos(curRoom.position);
                    if (roomPositions.ContainsKey(newRoomPos) || !config.bounds.Contains(newRoomPos))
                        continue;
                    var newRoom = new RoomSpawnData(curRoom.distanceFromStart + 1, newRoomPos);
                    rooms.Add(newRoom);
                    roomPositions[newRoomPos] = newRoom;
                    roomsPlanned++;
                    break;
                }
                curIndex++;
                emergencyExit++;
            }
        }

        private void SetRoomTypes()
        {
            var distantRoom = roomPositions.Values.OrderByDescending(it => it.distanceFromStart)
                .First();
            distantRoom.roomType = RoomType.End;
        }

        private Dictionary<Vector3Int, Room> SpawnRooms(Transform parent)
        {
            var result = new Dictionary<Vector3Int, Room>();
            foreach (var (pos, data) in roomPositions)
            {
                var roomConfig = new RoomConstructionConfig(
                    CalcDoorsDirections(pos),
                    globalGrid.CellToWorld(pos),
                    parent,
                    data.roomType,
                    data.distanceFromStart
                );
                var room = roomFactory.CreateRoom(roomConfig);
                result[pos] = room;
            }

            return result;
        }
        
        private WallDirection[] CalcDoorsDirections(Vector3Int pos)
        {
            var directions = new List<WallDirection>();
            if (roomPositions.ContainsKey(pos + Vector3Int.up))
                directions.Add(WallDirection.Top);
            if (roomPositions.ContainsKey(pos + Vector3Int.down))
                directions.Add(WallDirection.Bottom);
            if (roomPositions.ContainsKey(pos + Vector3Int.left))
                directions.Add(WallDirection.Left);
            if (roomPositions.ContainsKey(pos + Vector3Int.right))
                directions.Add(WallDirection.Right);
            return directions.ToArray();
        }
        
        private Vector3Int GetRandomNeighborPos(Vector3Int currentPos)
        {
            var delta = new int[] {-1, 1};
            var horizontal = Random.Range(0, 2) > 0;
            if (horizontal)
                return currentPos + new Vector3Int(delta[Random.Range(0, 2)], 0, 0);
            else
                return currentPos + new Vector3Int(0, delta[Random.Range(0, 2)], 0);
        }

        private class RoomSpawnData
        {
            public RoomType roomType;
            public int distanceFromStart;
            public Vector3Int position;
            
            public RoomSpawnData(int distanceFromStart, Vector3Int position, RoomType roomType = RoomType.Common)
            {
                this.position = position;
                this.roomType = roomType;
                this.distanceFromStart = distanceFromStart;
            }
        }
    }
}