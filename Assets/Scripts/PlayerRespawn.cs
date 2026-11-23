// This script is in charge of teleporting the player to whichever respawn point is current. 
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{ 
    [SerializeField] Transform respawnPoint;
    Rigidbody2D rb;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    // Tutorial moves the respawn forward as you make progress. 
    public void SetRespawnPoint(Transform point) => respawnPoint = point;

    public void Respawn()
    {
        transform.position = respawnPoint.position;
        rb.linearVelocity = Vector2.zero; 
    }
}
