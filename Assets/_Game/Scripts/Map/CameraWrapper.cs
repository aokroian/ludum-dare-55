
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
        
        
    }
}