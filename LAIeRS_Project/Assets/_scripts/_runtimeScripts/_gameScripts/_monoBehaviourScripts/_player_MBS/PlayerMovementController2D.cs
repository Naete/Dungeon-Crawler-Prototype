using UnityEngine;

namespace LAIeRS.Player
{
    [DisallowMultipleComponent, RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementController2D : MonoBehaviour
    {
        [SerializeField] private int _maxWalkSpeed = 10;
        [SerializeField] private float _accelerationTime = 1;
        [SerializeField] private float _decelerationTime = 1;
        private float _lastHighestSpeed = 0;
    
        private Vector2 _walkDirection;
        
        private Rigidbody2D _rigidBody2DComponent;
    
        private void Awake()
        { 
            _rigidBody2DComponent = GetComponent<Rigidbody2D>();
        }
        
        private void Update()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            
            _walkDirection = new Vector2(x, y).normalized;
        }
        
        private void FixedUpdate()
        { 
            _rigidBody2DComponent.velocity = _walkDirection * _maxWalkSpeed;
        }
    }
}