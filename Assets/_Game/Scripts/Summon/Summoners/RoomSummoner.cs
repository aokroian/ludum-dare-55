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
        
        public override void SummonAsync(SummonParams summonParams)
        {
            SummonRoom(summonParams);
        }

        private void SummonRoom(SummonParams summonParams)
        {
            var prefabIndex = Mathf.Min(_objectsHolder.rooms.Count, prefabs.Count - 1);

            var position = CalcRoomPosition();
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