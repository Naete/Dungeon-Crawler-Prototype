using System.ComponentModel;
using System.Threading.Tasks;

using UnityEngine;
using Random = UnityEngine.Random;

using LAIeRS.Player;

namespace LAIeRS.Miscellanious
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
    public class CharacterMovementController2D : MonoBehaviour
    {
        [Header("General Settings")]
        [SerializeField] private float _movementSpeed = 5;
        [SerializeField] private LayerMask _objectsToCollideWith;
        
        [Header("'Follow Target' Settings")]
        [SerializeField] private Transform _target;

        [Header("'Roaming Around' Settings")] 
        [SerializeField, Range(0, 10)] private float _roamRadius = 2;
        [SerializeField, Range(0, 5), Description("In Seconds")] private float _timeToWait = 2; // TODO: Randomize wait time
        [SerializeField, Range(0, 1)] private float _chanceToWait = 0.5f;

#if UNITY_EDITOR
        [Header("Monitoring")]
        [SerializeField, NaughtyAttributes.ReadOnly] private float _remainingTimeToWait = 0;
#endif
        
        private Rigidbody2D _rigidbody2DComponent;

        private Task _currentTask;
        private bool _cancelTask = false;
        
        private void Awake()
        {
            _rigidbody2DComponent = GetComponent<Rigidbody2D>();

            if (!_target) _target = FindObjectOfType<PlayerIdentifier>().transform;
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (!other.collider.GetComponent<PlayerIdentifier>())
                _cancelTask = true;
        }

        private void Update()
        {
            Vector2 currentPos = transform.position;
            Vector2 targetPos = _target.position;
            
            Vector2 direction = (targetPos - currentPos).normalized;
            float distance = Vector2.Distance(currentPos, targetPos);
            
            RaycastHit2D hitInfo = Physics2D.Raycast(currentPos, direction, distance, _objectsToCollideWith);
            
#if UNITY_EDITOR
            Debug.DrawLine(currentPos, hitInfo.point);
#endif
            
            if (hitInfo.transform.GetComponent<PlayerIdentifier>())
            {
                _cancelTask = true;
                MoveTowards(direction, _movementSpeed);
            }
            // TODO: Before "roaming" find a path to the target. If there is no possible path then start roaming 
            else if (_currentTask == null || _currentTask.IsCompleted)
            {
                _cancelTask = false;

                if (Random.value < _chanceToWait)
                    _currentTask = WaitingAsync(_timeToWait);
                else
                    _currentTask = RoamingAroundAsync(_roamRadius, _movementSpeed, _objectsToCollideWith);
            }
        }

        // TODO: Async task keeps running when exiting play mode
        private async Task RoamingAroundAsync(float roamRadius, float movementSpeed, LayerMask objectsToCollideWith)
        {
            Vector2 currentPos = transform.position;

            RaycastHit2D hitInfo;
            Vector2 direction;
            Vector2 targetPos;
            
            do {
                targetPos = Random.insideUnitCircle * roamRadius + currentPos;

                direction = (targetPos - currentPos).normalized;
                float distance = Vector2.Distance(currentPos, targetPos);

                hitInfo = Physics2D.Raycast(currentPos, direction, distance, objectsToCollideWith);
            } while (hitInfo);
            
            MoveTowards(direction, movementSpeed);
            
            while (!_cancelTask && Vector2.Distance(currentPos, targetPos) > 0.1f)
            {
#if UNITY_EDITOR
                Debug.DrawLine(currentPos, targetPos);
#endif
                currentPos = transform.position;

                await Task.Yield();
            }
            
            _rigidbody2DComponent.velocity = Vector2.zero;
        }

        private async Task WaitingAsync(float timeToWait)
        {
            float endOfWaitTime = Time.unscaledTime + timeToWait;
            while (!_cancelTask && Time.unscaledTime < endOfWaitTime)
            {
                _rigidbody2DComponent.velocity = Vector2.zero;
#if UNITY_EDITOR
                _remainingTimeToWait = endOfWaitTime - Time.unscaledTime;
#endif
                await Task.Yield();
            }

#if UNITY_EDITOR
            _remainingTimeToWait = 0;
#endif
        }
        
        private void MoveTowards(Vector2 direction, float movementSpeed)
        {
            _rigidbody2DComponent.velocity = direction * movementSpeed;
        }
    }
}