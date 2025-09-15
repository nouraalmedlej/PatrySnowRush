using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    public float speed = 7f;
    public float jumpForce = 12f;

    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundMask;

    public SpriteRenderer sprite;
    public Animator anim;

    Rigidbody2D rb;
    Vector2 move;
    bool jumpPressed;
    bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!sprite) sprite = GetComponent<SpriteRenderer>();
        if (!anim) anim = GetComponent<Animator>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck ? groundCheck.position : transform.position + Vector3.down * 0.5f,
            groundRadius, groundMask);

        if (anim)
        {
            anim.SetBool("walk", Mathf.Abs(move.x) > 0.01f);
            anim.SetBool("jump", !isGrounded);
        }

        if (sprite)
        {
            if (move.x < -0.01f) sprite.flipX = true;
            else if (move.x > 0.01f) sprite.flipX = false;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(move.x * speed, rb.linearVelocity.y);
        if (jumpPressed && isGrounded) rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpPressed = false;
    }

    public void OnMove(InputValue v)
    {
        float x = v.Get<float>();
        move = new Vector2(x, 0f);
        Debug.Log($"{name} OnMove = {x}");
    }

    public void OnJump()
    {
        jumpPressed = true;
        Debug.Log($"{name} OnJump");
    }
    public void OnJump(InputAction.CallbackContext c) { if (c.performed) jumpPressed = true; }
}
