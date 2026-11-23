// This script is in charge of the kill zone and teleporting the player if they fall into the pit. 
using UnityEngine;

public class KillZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerRespawn player))
            player.Respawn();
    }
}