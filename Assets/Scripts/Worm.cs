// Controls the evil cruel worm
using UnityEngine;

public enum WormState { Idle, Chasing, InMud, Sucked }

public class Worm : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] CoinCounter coins;
    [SerializeField] float detectRange = 14f;
    [SerializeField] float walkSpeed = 1.5f;
    [SerializeField] float muddyMultiplier = 3f;

    public WormState State { get; private set; } = WormState.Idle;
    // The tutorial makes the worm able to move with this:
    public bool CanAct { get; set; }
    // The tutorial makes the worm UNABLE to move with this:
    public bool Frozen { get; set; }

    Rigidbody2D rb; 
    Vector3 startPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (!CanAct || Frozen || State == WormState.Sucked)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float distance = Mathf.Abs(player.position.x - transform.position.x);
        if (State == WormState.Idle && distance < detectRange) State = WormState.Chasing;

        if (State == WormState.Idle)
        {
            rb.linearVelocity= Vector2.zero;
            return;
        }

        float speed = walkSpeed * (State == WormState.InMud ? muddyMultiplier : 1f);
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(direction * speed, 0f);
    }

    // This starts the worm movement without waiting for the player to get closer
    public void Alert()
    {
        if (State == WormState.Idle) State = WormState.Chasing;
    }

    public void Mud()
    {
        if (State != WormState.Sucked) State = WormState.InMud;
    }

    // This function tries sucking up the worm and adding a coin to your coin counter, however, only a worm covered in mud may be sucked up 
    public void TrySuck()
    {
        if (State != WormState.InMud) return;
        State = WormState.Sucked; 
        coins.Add(1);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (State == WormState.Sucked) return;
        if (!other.TryGetComponent(out PlayerRespawn playerRespawn)) return;
        playerRespawn.Respawn();
        // This puts the worm back, so the player can try again
        transform.position = startPosition;
        State = WormState.Idle;
    }
}