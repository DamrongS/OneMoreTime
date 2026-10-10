using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Rigidbody2D _rigidBody2d;
    BoxCollider2D _groundTrigger;
    private InputAction _move;
    private InputAction _jump;
    private bool _grounded;
    private bool _colliding;
    private float coyote_time = 0.25f;
    private float coyote_timer = 0;
    private bool falling;
    public int _speed;
    public int jumpPower;
    public float damping;
    void Start()
    {
        _rigidBody2d = GetComponent<Rigidbody2D>();
        _groundTrigger = GetComponent<BoxCollider2D>();
        _move = InputSystem.actions.FindAction("Move");
        _jump = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dt = Time.deltaTime;

        // Definitions
        Vector2 move = _move.ReadValue<Vector2>();
        float jump = _jump.ReadValue<float>();

        bool falling = math.abs(_rigidBody2d.linearVelocityY) > 0;
        bool jumping = jump > 0 && _grounded && !falling;

        // Coyote time handler
        if(falling)
        {
            coyote_timer += dt;
            if(coyote_timer > coyote_time)
            {
                _grounded = false;
            }
        }
        else
        {
            coyote_timer = 0;
            _grounded = true;
        }

        // Jumping input check
        if(jumping)
        {
            _rigidBody2d.AddForce(new Vector2(0, jumpPower));
            _grounded = false;
        }

        _rigidBody2d.linearVelocity += move * new Vector2(dt * _speed, 0);
        _rigidBody2d.linearVelocityX *= damping;

    }
}