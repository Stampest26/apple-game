// This script controls the pieces of nectar (collectibles for progression)!
using UnityEngine;

public class Nectar : MonoBehaviour
{
    [SerializeField] CoinCounter nectarCounter;
    [SerializeField] float bobHeight = 0.3f;
    [SerializeField] float bobSpeed = 2f; 

    public bool Collected { get; private set; }
    Vector3 startPosition;

    void Awake() => startPosition = transform.position;

    void Update()
    {
        // If the nectar is collected it does not exist in the map basically, you cannot collect it again.
        if (Collected) return;
        // Controls the floating 
        transform.position = startPosition + Vector3.up * Mathf.Sin(Time.time * bobSpeed) * bobHeight;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (Collected) return;
        if (!other.TryGetComponent(out PlayerController _)) return;

        // When you touch it, it is collected,
        Collected = true;
        // And you add one nectar to your nectar counter
        nectarCounter.Add(1);
        // And remove the nectar
        gameObject.SetActive(false);
    }
}