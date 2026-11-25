// Controls the evil cruel worm
using UnityEngine;
using System.Collections;

public enum WormState { Idle, Chasing, InMud, Sucked }

public class Worm : MonoBehaviour
{
    // Instantiates all of the stuff here
    [SerializeField] Transform player;
    [SerializeField] CoinCounter coins;
    [SerializeField] float detectRange = 14f;
    [SerializeField] float walkSpeed = 1.5f;
    [SerializeField] float muddyMultiplier = 3f;
    [SerializeField] Color mudColor = new Color(0.45f, 0.28f, 0.13f);
    [SerializeField] float tinyScale = 0.2f;
    [SerializeField] float shrinkTime = 0.15f;
    [SerializeField] float flySpeed = 30f;

    public WormState State { get; private set; } = WormState.Idle;
    // The tutorial makes the worm able to move with this:
    public bool CanAct { get; set; }
    // The tutorial makes the worm UNABLE to move with this:
    public bool Frozen { get; set; }
    public bool Collected { get; private set; }
    public event System.Action PlayerKilled; 

    Rigidbody2D rb; 
    Vector3 startPosition;
    SpriteRenderer sprite;
    Color startColor;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        sprite = GetComponent<SpriteRenderer>();
        startColor = sprite.color;
    }

    void FixedUpdate()
    {
        // Basically immediately says that if the worm is sucked, it's gone, treat it as if it's gone 
        if (State == WormState.Sucked) return;

        // If it can't do anything, it has no velocity
        if (!CanAct || Frozen)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        float distance = Mathf.Abs(player.position.x - transform.position.x);
        if (State == WormState.Idle && distance < detectRange) State = WormState.Chasing;

        if (State == WormState.Idle)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        float speed = walkSpeed * (State == WormState.InMud ? muddyMultiplier : 1f);
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }

    public void Alert()
    {
        if (State == WormState.Idle) State = WormState.Chasing;
    }

    public void Mud()
    {
        if (State == WormState.Sucked) return;
        State = WormState.InMud;
        sprite.color = mudColor;
    }

    public void TrySuck(Transform suckPoint)
    {
        // If the worm is not in mud, do not try to suck it up
        if (State != WormState.InMud) return;
        State = WormState.Sucked; 
        StartCoroutine(FlyIntoSucker(suckPoint));
    }

    IEnumerator FlyIntoSucker(Transform suckPoint)
    {
        rb.simulated = false; 
        Vector3 startScale = transform.localScale;
        Vector3 smallScale = startScale * tinyScale; 
        float shrinkProgress = 0f;

        while (Vector2.Distance(transform.position, suckPoint.position) > 0.15f)
        {
            shrinkProgress += Time.deltaTime / shrinkTime;
            transform.localScale = Vector3.Lerp(startScale, smallScale, shrinkProgress);
            transform.position = Vector3.MoveTowards(transform.position, suckPoint.position, flySpeed * Time.deltaTime);
            yield return null;
        }

        coins.Add(1);
        Collected = true;
        gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (State == WormState.Sucked) return;
        if (!collision.collider.TryGetComponent(out PlayerRespawn playerRespawn)) return;

        playerRespawn.Respawn();
        transform.position = startPosition;
        rb.linearVelocity = Vector2.zero;
        sprite.color = startColor;
        State = WormState.Idle;
        PlayerKilled?.Invoke();

    }
}