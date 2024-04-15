
using System.Threading.Tasks;
using UnityEngine;

namespace _Game.Scripts.Map
{
    public class CameraWrapper: MonoBehaviour
    {
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
    }
}