using System;
using System.Threading.Tasks;
using _Game.Scripts.Summon.View;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Game.Scripts.Summon.Summoners
{
    public class RoomSummoner: Summoner
    {
        [SerializeField] private Transform roomParent;
        [SerializeField] private float roomMargin;
        
        // public override async Task SummonAsync(SummonParams summonParams)
        // {
        //     var startPosition = summonParams._camera.transform.position;
        //     var position = CalcRoomPosition();
        //     await MoveCameraToAsync(summonParams._camera, position);
        //     SummonRoom(position);
        //     await MoveCameraToAsync(summonParams._camera, startPosition);
        // }
        
        public override async Task Summon(SummonParams summonParams)
        {
            // TODO: try..catch. We need to enable input even if something goes wrong
            var startPosition = summonParams._camera.transform.position;
            var position = CalcRoomPosition();
            await MoveCameraToAsync(summonParams._camera, position);
            SummonRoom(position);
            await MoveCameraToAsync(summonParams._camera, startPosition);
        }

        private void SummonRoom(Vector3 position)
        {
            var prefabIndex = Mathf.Min(_objectsHolder.rooms.Count, prefabs.Count - 1);

            var room = Instantiate(prefabs[prefabIndex], position, Quaternion.identity, roomParent);
            _objectsHolder.AddRoom(room as SummonedRoom);
        }

        private Vector3 CalcRoomPosition()
        {
            var tileMap = prefabs[0].GetComponentInChildren<Tilemap>();
            var roomSize = tileMap.cellBounds.size.y * tileMap.cellSize.y;
            var roomPosition = (roomSize + roomMargin) * _objectsHolder.rooms.Count * Vector3.up;
            return roomPosition;
        }
    }
}