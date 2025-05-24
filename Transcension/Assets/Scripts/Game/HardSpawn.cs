using UnityEngine;

public class HardSpawn : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player")) // When player collides with hard spawn, we call the hardSpawn method that changes the the player's spawnpoint to this hard spawn.
        {
            playerMovement.hardSpawn();
        }
    }
}
