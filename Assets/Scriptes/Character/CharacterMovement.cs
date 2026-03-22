using System;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _fallMultiplier;
    [SerializeField] private float _lowJumpMultiplier;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundCheck;

    [Header("Platforms modificator")]
    [SerializeField] private float resistanceMultiplier = 0.5f;
    [SerializeField] private float boostMultiplier = 2f;

    private Rigidbody2D _rigidBody;
    private Animator _animator;
    private SpriteRenderer _renderer;
    private BoxCollider2D boxCollider2D;
    private float horizontalInput;
    private bool onSpeedPlatform;
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    private void FixedUpdate()
    {
        float currentSpeed = _speed;
        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 0.6f, _groundLayer);

        if (hit.collider != null && hit.collider.GetComponent<GoPlatforms>())
        {
            onSpeedPlatform = true;
        }
        else
        {
            onSpeedPlatform = false;
        }

        if (onSpeedPlatform)
        {
            if (horizontalInput > 0.1f)
            {
                currentSpeed *= resistanceMultiplier; // замедление
            }
            else if (horizontalInput < -0.1f)
            {
                currentSpeed *= boostMultiplier; // ускорение
            }
        }

        _rigidBody.linearVelocity = new Vector2(horizontalInput * currentSpeed, _rigidBody.linearVelocity.y);
    }
    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        _rigidBody.linearVelocity = new Vector2(horizontalInput * _speed, _rigidBody.linearVelocity.y);
        if (!isGrounded() && !Input.GetKey(KeyCode.Space)) {
            _animator.SetTrigger("Fall");
        }

        // Flip player when moving right to left and left to right
        if (horizontalInput > 0.01f)
        {
            //transform.localScale = new Vector3(5, 5, 1);
            _renderer.flipX = false;
        }
        else if (horizontalInput < -0.01f)
        {
            //transform.localScale = new Vector3(-5, 5, 1); 
            _renderer.flipX = true;
        }
        if (Input.GetKey(KeyCode.Space) && isGrounded())
        {
            Jump();
        }
        if (_rigidBody.linearVelocity.y < 0)
        {
            _rigidBody.linearVelocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;
        }
        else if (_rigidBody.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            _rigidBody.linearVelocity += Vector2.up * Physics2D.gravity.y * (_lowJumpMultiplier - 1) * Time.deltaTime;
        }

        if (!isGrounded() && _rigidBody.linearVelocity.y < -0.1f)
        {
            _animator.SetBool("FallDown", true);
        }
        else
        {
            _animator.SetBool("FallDown", false);
        }

        _animator.SetBool("IsMoving", horizontalInput != 0);
        _animator.SetBool("Grounded", isGrounded());
    }

    private void Jump()
    {
        _rigidBody.linearVelocity = new Vector2(_rigidBody.linearVelocity.x, _jumpForce);
        _animator.SetTrigger("Jump");
    }
    private bool isGrounded()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, Vector2.down, 0.1f,_groundLayer);
        return raycastHit2D.collider != null;
    }
}