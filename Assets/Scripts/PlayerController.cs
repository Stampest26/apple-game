// This script deals with the controls of the player 
// Lets script know what to use 
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour 
{
    // Basically instantiating stuff
    [SerializeField] float moveSpeed = 7f;
    [SerializeField] float jumpForce = 12f;
    [SerializeField] float groundRadius = 0.15f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    // 1 means he's going right, -1 means he's going left
    public int Facing { get; private set; } = 1;

    // This lets the tutorial freeze the guy
    public bool InputLocked {get; set;}

    Rigidbody2D rb;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void Update()
    {
        // The kb stands for a keyboard :)
        // This is where I called the piechart goofy
        var kb = Keyboard.current;
        float x = 0f;
        // If you're NOT input locked, 
        if (!InputLocked) 
        {
            // Basically lets it know that when you press the A key (WASD controls) or left arrow (arrow controls) you move left!
            if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) x -= 1f;
            // Same thing as before, but the other way 
            if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) x += 1f;
        }

        if (x != 0f)
        {
            Facing = x > 0 ? 1 : -1;
            transform.localScale = new Vector3(Facing, 1, 1);
        }

        // Basically calculates the linear velocity 
        rb.linearVelocity = new Vector2(x * moveSpeed, rb.linearVelocity.y);

        bool grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        if (!InputLocked && grounded && kb.spaceKey.wasPressedThisFrame)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }


}