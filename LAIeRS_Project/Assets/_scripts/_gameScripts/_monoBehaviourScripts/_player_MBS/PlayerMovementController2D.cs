using UnityEngine;

namespace LAIeRS.Player
{
    [DisallowMultipleComponent, RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementController2D : MonoBehaviour
    {
        [SerializeField] private int maxWalkSpeed = 10;
    
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
            _rigidBody2DComponent.velocity = _walkDirection * maxWalkSpeed;
        }
    }
}