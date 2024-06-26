﻿using System;
using System.Threading.Tasks;
using _Game.Scripts.Summon.View;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Game.Scripts.Summon.Summoners
{
    public class RoomSummoner: Summoner
    {
        [SerializeField] private Transform roomParent;
        [SerializeField] private float roomMargin;
        
        public override async Task Summon(SummonParams summonParams)
        {
            try
            {
                await SummonInner(summonParams);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private async Task SummonInner(SummonParams summonParams)
        {
            var startPosition = summonParams.camera.transform.position;
            var position = CalcRoomPosition();
            var isFirstRoom = _objectsHolder.RealRoomCount == 0;
            
            if (!isFirstRoom)
                await MoveCameraToAsync(summonParams.camera, position);
            
            await UniTask.Delay(100);
            var room = SummonRoom(position);
            Summoned(room);
            
            // TODO: Animate spawn instead
            await UniTask.Delay(300);
            
            if (!isFirstRoom)
                await MoveCameraToAsync(summonParams.camera, startPosition);
        }

        private SummonedRoom SummonRoom(Vector3 position)
        {
            var prefabIndex = Mathf.Min(_objectsHolder.RealRoomCount, prefabs.Count - 1);

            var room = _diContainer.InstantiatePrefabForComponent<SummonedRoom>(prefabs[prefabIndex],
                position,
                Quaternion.identity,
                roomParent);
            _objectsHolder.AddRoom(room);
            return room;
        }

        private Vector3 CalcRoomPosition()
        {
            var tileMap = prefabs[0].GetComponentInChildren<Tilemap>();
            var roomSize = tileMap.cellBounds.size.y * tileMap.cellSize.y;
            var roomPosition = (roomSize + roomMargin) * _objectsHolder.RealRoomCount * Vector3.up;
            return roomPosition;
        }
    }
}