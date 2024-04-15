using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.Story.Characters.Princess
{
    public class PrincessWandering: MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        [SerializeField] private float maxStayTime = 2f;
        [SerializeField] private float maxDistance = 1f;
        
        private Collider2D _walkingArea;
        private bool _isPaused = true;
        
        private Vector3 _targetPosition;
        private float _startMoveTime;
        private bool _isMoving;

        public void Init(Collider2D walkingArea)
        {
            _walkingArea = walkingArea;
            _isPaused = false;
        }

        private void Update()
        {
            if (_isPaused)
                return;

            if (_isMoving)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, _targetPosition) < 0.3f)
                {
                    _isMoving = false;
                    _startMoveTime = Time.time + Random.Range(0.5f, maxStayTime);
                }
            }
            else
            {
                if (Time.time > _startMoveTime)
                {
                    _targetPosition = GetRandomPositionWithinWalkingArea();
                    _isMoving = true;
                }
            }
        }
        
        private Vector3 GetRandomPositionWithinWalkingArea()
        {
            Bounds bounds = _walkingArea.bounds;
            return new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                transform.position.z
            );
        }

        public void TogglePause(bool paused)
        {
            _isPaused = paused;
        }
    }
}