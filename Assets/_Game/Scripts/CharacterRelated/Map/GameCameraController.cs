using System;
using DG.Tweening;
using Map.Model;
using UnityEngine;

namespace Map
{
    public class GameCameraController : MonoBehaviour
    {
        private Camera _camera;
        private float _zPos;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _zPos = transform.position.z;
        }

        public void MoveToRoom(Room room, float animationTime = 0f)
        {
            var correction = new Vector3(0.5f, 1.2f, 0f);
            var cameraPos = new Vector3(room.center.x, room.center.y, _zPos) + correction;
            _camera.transform.DOMove(cameraPos, animationTime).SetEase(Ease.InOutCubic);
        }
    }
}