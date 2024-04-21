using System.Linq;
using System.Threading.Tasks;
using _Game.Scripts.Common;
using _Game.Scripts.Summon.Data;
using _Game.Scripts.Summon.View;
using UnityEngine;

namespace _Game.Scripts.Map
{
    public class RoomSwitcher
    {
        private SummonedObjectsHolder _objectsHolder;
        private CameraWrapper _cameraWrapper;
        private GlobalInputSwitcher _globalInputSwitcher;

        public RoomSwitcher(SummonedObjectsHolder objectsHolder, CameraWrapper cameraWrapper,
            GlobalInputSwitcher globalInputSwitcher)
        {
            _objectsHolder = objectsHolder;
            _cameraWrapper = cameraWrapper;
            _globalInputSwitcher = globalInputSwitcher;
        }

        public async Task SwitchRoom(bool forward)
        {
            var roomIndex = _objectsHolder.GetPlayerRoomIndex();
            if (roomIndex == -1)
            {
                Debug.LogError("Player is not in any room");
                return;
            }

            var room = _objectsHolder.Rooms[roomIndex];
            var nextRoomIndex = forward ? roomIndex + 1 : roomIndex - 1;
            if (nextRoomIndex < 0 || nextRoomIndex >= _objectsHolder.Rooms.Count)
            {
                await ToEmptyRoom(room, forward);
            }
            else
            {
                await ToRoomExists(room, _objectsHolder.Rooms[nextRoomIndex], forward);
            }
        }

        private async Task ToRoomExists(SummonedRoom prevRoom, SummonedRoom room, bool forward)
        {
            _globalInputSwitcher.DisableAllInput();
            await _cameraWrapper.MoveCameraToAsync(room.transform.position);
            var player = _objectsHolder.GetPlayer();
            prevRoom.RemoveObject(player);
            room.AddObject(player);
            player.transform.position = forward ? room.entrance.position : room.exit.position;
            _globalInputSwitcher.EnableAllInput();

            // move bard to the same room
            var bard = _objectsHolder.GetSummonedObjectsOfType<SummonedBard>()?
                .FirstOrDefault(b => b != null && !b.IsKillOneselfScheduled);
            if (bard != null)
            {
                prevRoom.RemoveObject(bard);
                room.AddObject(bard);
                bard.transform.position = forward ? room.entrance.position : room.exit.position;
            }
        }

        private async Task ToEmptyRoom(SummonedRoom prevRoom, bool forward)
        {
            _globalInputSwitcher.DisableAllInput();
            var player = _objectsHolder.GetPlayer();
            var newPosition = prevRoom.transform.position + (forward ? Vector3.up : Vector3.down) * 10;
            player.transform.position = newPosition;
            await _cameraWrapper.MoveCameraToAsync(newPosition);
            prevRoom.RemoveObject(player);
            _objectsHolder.ObjectsOutOfRoom.Add(player);
        }
    }
}