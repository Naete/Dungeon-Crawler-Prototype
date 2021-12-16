using System;
using System.Threading.Tasks;
using LAIeRS.ExtensiveMethods;
using UnityEngine;
using Random = UnityEngine.Random;

using LAIeRS.Player;

namespace LAIeRS.Miscellanious
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
    public class AIMovementController2D : MonoBehaviour
    {
        [Header("General Settings")]
        [SerializeField] private float _maxMovementSpeed = 5;
        [SerializeField, Range(0.1f, 50)] private float _accelerationTime = 1;
        [SerializeField, Range(0.1f, 50)] private float _decelerationTime = 1;
        [SerializeField] private LayerMask _objectsToCollideWith;
        [SerializeField] private bool _showGizmo;
        
        [Header("'Follow' Settings")]
        [SerializeField] private Transform _target;
        [SerializeField] private float _followRadius = 2;
        
        [Header("'Roam' Settings")] 
        [SerializeField] private float _roamRadius = 2;
        [SerializeField] private float _timeToWait = 2; // TODO: Randomize wait time
        [SerializeField, Range(0, 1)] private float _chanceToWait = 0.5f; // TODO: Randomize chance to wait

        [Header("Monitoring")] 
        [SerializeField] private Vector2 _currentTargetPos;
        [SerializeField] private Vector2 _currentDirection;
        
        private Rigidbody2D _rigidbody2DComponent;

        private Task _currentTask;
        private bool _cancelTask;
        
        private void Awake()
        {
            // TODO: Use PlayerDataController to get player
            if (!_target) _target = FindObjectOfType<PlayerIdentifier>().transform;
            
            _rigidbody2DComponent = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (!other.collider.GetComponent<PlayerIdentifier>())
                _cancelTask = true;
        }

        private void OnDisable()
        {
            _cancelTask = true;
        }
        
        private void Update()
        {
            RaycastHit2D hitData = ExecuteRaycast(_target.position, out Vector2 directionOfTarget, _objectsToCollideWith);
            
            // TODO: When moving too fast out of the ais follow radius, it will keep accelerating until the task is done
            if (hitData.transform == _target && Vector2.Distance(transform.position, _target.position) < _followRadius)
            {
                if (IsNotWalking()) 
                    _currentTask = AccelerateAsync(_target.position, _maxMovementSpeed);
                
                _currentTargetPos = _target.position;
                _currentDirection = _rigidbody2DComponent.velocity.normalized;
                
                ChangeDirectionTo(directionOfTarget);
            }
            else if (IsTaskDone(_currentTask))
            {
                _currentTask = Random.value < _chanceToWait 
                    ? WaitAsync(_timeToWait) 
                    : RoamingAroundAsync(_roamRadius, _maxMovementSpeed);
            }

            if (_showGizmo)
            {
                Visualizer.DrawLine(transform.position, _currentTargetPos);
                Visualizer.DrawCircle(_currentTargetPos, _decelerationTime, Color.yellow);
                Visualizer.DrawCircle(transform.position, _roamRadius);
                Visualizer.DrawCircle(transform.position, _followRadius, Color.red);
            }
        }
        
        private bool IsNotWalking()
        {
            return _rigidbody2DComponent.velocity.magnitude <= 0;
        }

        private void ChangeDirectionTo(Vector2 direction)
        { 
            _rigidbody2DComponent.velocity = direction.normalized * _rigidbody2DComponent.velocity.magnitude;
        }
        
        private bool IsTaskDone(Task task)
        {
            if (task == null || task.IsCompleted)
            {
                task?.Dispose();
                return true;
            }
            
            return false;
        }
        
        private async Task WaitAsync(float duration)
        {
            _cancelTask = false;
            
            float endOfWaitTime = Time.unscaledTime + duration;

            _rigidbody2DComponent.velocity = Vector2.zero;

            while (!_cancelTask && Time.unscaledTime < endOfWaitTime)
                await Task.Yield();
            
            _cancelTask = false;
        }

        private async Task RoamingAroundAsync(float radius, float maxMovementSpeed)
        {
            _cancelTask = false;

            Vector2 targetPos = GetRandomPosInRadius(radius, transform);

            _currentTargetPos = targetPos;
            _currentDirection = _rigidbody2DComponent.velocity.normalized;
            
            await AccelerateAsync(targetPos, maxMovementSpeed);
    
            while (Vector2.Distance(transform.position, targetPos) > _decelerationTime)
            {
                if (_cancelTask)
                {
                    _rigidbody2DComponent.velocity = Vector2.zero;
                    return;
                }
                
                await Task.Yield();
            }
            
            await DecelerateAsync(_rigidbody2DComponent.velocity.normalized);
        }
        
        private RaycastHit2D ExecuteRaycast(Vector2 targetPos, out Vector2 direction, LayerMask layerMask)
        {
            Vector2 currentPos = transform.position;

            direction = currentPos.GetDirectionTo(targetPos);
            float distance = currentPos.GetDistanceTo(targetPos);

            RaycastHit2D hitData = Physics2D.Raycast(currentPos, direction, distance, layerMask);
            
            if (_showGizmo)
                Visualizer.DrawLine(currentPos, hitData.point);

            return hitData;
        }
        
        private Vector2 GetRandomPosInRadius(float radius, Transform current)
        {
            Vector2 targetPos;
            Vector2 directionOfTarget;
            
            do targetPos = Random.insideUnitCircle * radius + (Vector2)current.position;
            while (ExecuteRaycast(targetPos, out directionOfTarget, _objectsToCollideWith));

            return targetPos;
        }
        
        private async Task AccelerateAsync(Vector2 targetPos, float maxMovementSpeed)
        {
            _cancelTask = false;
            
            float timeToEndAcceleration = Time.unscaledTime + _accelerationTime;
            
            Vector2 direction = transform.position.GetDirectionTo(targetPos);
            
            _rigidbody2DComponent.velocity = direction;
            
            Vector2 accelerationSpeed = direction * (maxMovementSpeed / _accelerationTime);
            
            while (Time.unscaledTime < timeToEndAcceleration 
                   && Vector2.Distance(targetPos, transform.position) > 0.1f)
            {
                if (_cancelTask)
                {
                    _rigidbody2DComponent.velocity = Vector2.zero;
                    return;
                }
                
                _rigidbody2DComponent.velocity += accelerationSpeed * Time.deltaTime;
                
                await Task.Yield();
            }
            
            _cancelTask = false;
        }
        
        private async Task DecelerateAsync(Vector2 targetPos)
        {
            _cancelTask = false;
            
            float timeToEndDeceleration = Time.unscaledTime + _decelerationTime;
            
            Vector2 currentVelocity = _rigidbody2DComponent.velocity;
            Vector2 direction = currentVelocity.normalized;
            Vector2 decelerationSpeed = direction * currentVelocity.magnitude / _decelerationTime;
            
            while (Time.unscaledTime < timeToEndDeceleration 
                   && Vector2.Distance(targetPos, transform.position) > 0.01f)
            {
                if (_cancelTask)
                {
                    _rigidbody2DComponent.velocity = Vector2.zero;
                    return;
                }
                
                _rigidbody2DComponent.velocity -= decelerationSpeed * Time.deltaTime;
                
                await Task.Yield();
            }
            
            _rigidbody2DComponent.velocity = Vector2.zero;
            
            _cancelTask = false;
        }
    }
}