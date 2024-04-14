using System.Linq;
using Common;
using Map.Model;
using Map.Runtime;
using Map.Transitions;
using UnityEngine;

namespace Map
{
    public class RoomFactory: MonoBehaviour
    {
        [Header("Room parts prefabs")]
        [SerializeField] private GameObject ground;
        [SerializeField] private GameObject wallTop;
        [SerializeField] private GameObject wallTopDoor;
        [SerializeField] private GameObject wallRight;
        [SerializeField] private GameObject wallRightDoor;
        [SerializeField] private GameObject wallBottom;
        [SerializeField] private GameObject wallBottomDoor;
        [SerializeField] private GameObject wallLeft;
        [SerializeField] private GameObject wallLeftDoor;
        [SerializeField] private GameObject FloorEntry;
        [SerializeField] private GameObject FloorExit;
        [SerializeField] private GameObject npc;
            
        public Room CreateRoom(RoomConstructionConfig config)
        {
            var roomObj = new GameObject(GetRoomName(config));
            roomObj.transform.SetParent(config.parent);
            roomObj.transform.position = config.position;
            var room = roomObj.AddComponent<Room>();
            
            CreateGround(config, roomObj);
            CreateAllWalls(config, roomObj);
            CustomizeRoom(config, roomObj);
            
            room.distanceFromStart = config.distanceFromStart;
            
            room.InitializeRoom(config.roomType);
            return room;
        }
        
        private void CreateGround(RoomConstructionConfig config, GameObject room)
        {
            var ground = Instantiate(this.ground, config.position, Quaternion.identity, room.transform);
            // TODO: ground variations
        }
        
        private void CreateAllWalls(RoomConstructionConfig config, GameObject room)
        {
            if (config.doors.Contains(WallDirection.Top))
                Instantiate(wallTopDoor, config.position, Quaternion.identity, room.transform);
            else
                Instantiate(wallTop, config.position, Quaternion.identity, room.transform);
            
            if (config.doors.Contains(WallDirection.Right))
                Instantiate(wallRightDoor, config.position, Quaternion.identity, room.transform);
            else
                Instantiate(wallRight, config.position, Quaternion.identity, room.transform);
            
            if (config.doors.Contains(WallDirection.Bottom))
                Instantiate(wallBottomDoor, config.position, Quaternion.identity, room.transform);
            else
                Instantiate(wallBottom, config.position, Quaternion.identity, room.transform);
            
            if (config.doors.Contains(WallDirection.Left))
                Instantiate(wallLeftDoor, config.position, Quaternion.identity, room.transform);
            else
                Instantiate(wallLeft, config.position, Quaternion.identity, room.transform);
        }
        
        private string GetRoomName(RoomConstructionConfig config)
        {
            var name = "Room_";
            foreach (var door in config.doors)
            {
                name += door.ToString();
            }

            return name;
        }

        private void CustomizeRoom(RoomConstructionConfig config, GameObject room)
        {
            GameObject obj = null;
            if (config.roomType == RoomType.Start)
            {
                obj = Instantiate(FloorEntry, config.position, Quaternion.identity, room.transform);
                if (FloorController.Instance.CurrentDepth == 0)
                {
                    CreateNpc(config, room);
                }
            }
            else if (config.roomType == RoomType.End)
            {
                obj = Instantiate(FloorExit, config.position, Quaternion.identity, room.transform);
                CreateNpc(config, room);
            }

            var exitTrigger = obj?.GetComponentInChildren<FloorExit>();
            if (exitTrigger != null)
            {
                var depth = FloorController.Instance.CurrentDepth;
                exitTrigger.Initialize(depth);
            }
        }

        private void CreateNpc(RoomConstructionConfig config, GameObject room)
        {
            Instantiate(
                npc,
                config.position + npc.transform.position,
                Quaternion.identity,
                room.transform);
        }
    }
}