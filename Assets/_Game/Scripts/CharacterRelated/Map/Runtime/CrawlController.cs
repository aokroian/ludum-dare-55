using System.Linq;
using Actors;
using Actors.ActorSystems;
using Actors.InputThings;
using Actors.Spawn;
using Common;
using DG.Tweening;
using Game;
using Map.Model;
using UnityEngine;
using Utils;

namespace Map.Runtime
{
    public class CrawlController : SingletonScene<CrawlController>
    {
        [SerializeField] private float roomSwitchDuration = 1f;
        [SerializeField] private GameCameraController _cameraController;
        
        public Level currentLevel;

        private int enemiesLeft;
        private Room currentRoom;

        public void SetPlayerPosStairsRoom(PlayerActorInput player, bool downstairs)
        {
            var curRoom = downstairs
                ? currentLevel.rooms.Values.First(it => it.roomType == RoomType.Start)
                : currentLevel.rooms.Values.First(it => it.roomType == RoomType.End);
            
            var startPos = downstairs ? new Vector3(4, -3, 0) : new Vector3(4, 3, 0);
            
            var center = curRoom.center;
            player.transform.position = center + startPos;
            curRoom.FadeIn(0f);
            _cameraController.MoveToRoom(curRoom);
            
            player.ToggleInput(false);
            // player.gameObject.GetComponent<Collider2D>().enabled = false;
            
            player.transform.DOMove(center + new Vector3(4, 0, 0), 0.6f)
                .OnComplete(() => OnRoomEntered(player, curRoom));
        }
        
        public void ExitRoom(PlayerActorInput player, Room room, RoomExit exit)
        {
            player.ToggleInput(false);
            // player.gameObject.GetComponent<Collider2D>().enabled = false;
            
            var newRoom = currentLevel.rooms[currentLevel.GetRoomPosition(room) +
                                             CommonUtils.DirectionToVector(exit.Direction)];
            var newRoomEntrance = newRoom.entrances.First(
                it => it.Direction == CommonUtils.OppositeDirection(exit.Direction));
            
            OnRoomSwitchStarted(room, exit, newRoom, newRoomEntrance);

            player.transform.DOMove(newRoomEntrance.transform.position, roomSwitchDuration).SetEase(Ease.InOutCubic)
                .OnComplete(() => OnRoomEntered(player, newRoom));
        }

        private void OnRoomSwitchStarted(Room prevRoom, RoomExit exit, Room newRoom, RoomEntrance entrance)
        {
            newRoom.SetExitTriggersEnabled(false);
            _cameraController.MoveToRoom(newRoom, roomSwitchDuration);
            prevRoom.FadeOut(roomSwitchDuration);
            newRoom.FadeIn(roomSwitchDuration);
            
            if (!newRoom.visited && newRoom.roomType is RoomType.Common)
            {
                var enemies = SpawnUtil.SpawnEnemiesForRoom(newRoom, entrance.Direction);
                enemiesLeft = enemies.Count;
                foreach (var enemy in enemies)
                {
                    enemy.gameObject.GetComponent<ActorHealth>().OnDeath += EnemyEliminated;
                }
            }
        }

        private void OnRoomEntered(PlayerActorInput player, Room room)
        {
            currentRoom = room;
            player.ToggleInput(true);
            room.SetExitTriggersEnabled(true);
            // player.gameObject.GetComponent<Collider2D>().enabled = true;
            room.visited = true;

            if (enemiesLeft > 0)
            {
                room.CloseDoors();
            }

            if (!DialogueController.Instance.IntroShowed)
            {
                player.ToggleInput(false);
                DialogueController.Instance.Intro(() => player.ToggleInput(true));
            }

            if (room.roomType == RoomType.End &&
                PackageController.Instance.currentPackage?.receiverDepth ==
                FloorController.Instance.CurrentDepth)
            {
                player.ToggleInput(false);
                DialogueController.Instance.DeliverPackage(
                    PackageController.Instance.currentPackage,
                    () => player.ToggleInput(true));
            }
        }

        private void EnemyEliminated(ActorHealth actorHealth)
        {
            enemiesLeft--;
            if (enemiesLeft == 0)
            {
                currentRoom.OpenDoors();
            }
        }
    }
}