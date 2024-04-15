
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Map
{
    public class CameraWrapper: MonoBehaviour
    {
        [SerializeField] private float defaultSize = 7f;
        [SerializeField] private float zoomSize = 3f;
        
        private Camera _camera;
        
        private void Awake()
        {
            _camera = Camera.main;
        }
        
        public async Task MoveCameraToAsync(Vector3 position)
        {
            var cameraPosition = transform.position;
            var cameraTargetPosition = new Vector3(position.x, position.y, cameraPosition.z);
            var cameraMoveTime = 0.5f;
            var cameraMoveTimeElapsed = 0f;
            while (cameraMoveTimeElapsed < cameraMoveTime)
            {
                cameraMoveTimeElapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(cameraPosition, cameraTargetPosition, cameraMoveTimeElapsed / cameraMoveTime);
                await Task.Yield();
            }
        }

        public async Task ZoomIn(Vector3 position)
        {
            var yAddition = - _camera.transform.localPosition.y / 2;
            var cameraPosition = transform.position;
            var cameraTargetPosition = new Vector3(position.x, position.y + yAddition, cameraPosition.z);
            var cameraMoveTime = 0.5f;
            var cameraMoveTimeElapsed = 0f;
            while (cameraMoveTimeElapsed < cameraMoveTime)
            {
                cameraMoveTimeElapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(cameraPosition, cameraTargetPosition, cameraMoveTimeElapsed / cameraMoveTime);
                _camera.orthographicSize = Mathf.Lerp(defaultSize, zoomSize, cameraMoveTimeElapsed / cameraMoveTime);
                await Task.Yield();
            }
        }

        public void ResetCamera()
        {
            transform.position = new Vector3(0, 0, transform.position.z);
            _camera.orthographicSize = defaultSize;
        }
    }
}