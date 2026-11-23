// This controls the mud projectile! It is a blob of mud that you use to cover the worm in mud (so that it is 'InMud')
using UnityEngine;

public class MudProjectile : MonoBehaviour
{
    [SerializeField] float speed = 14f;
    [SerializeField] float lifetime = 3f;
    Rigidbody2D rb;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    public void Launch(Vector2 direction)
    {
        rb.linearVelocity = direction * speed;
        // Destroys it, forever (if it does not hit anything)
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // worm worm
        if (other.TryGetComponent(out Worm worm))
        {
            worm.Mud();
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}