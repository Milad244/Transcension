using UnityEngine;

public class HardSpawn : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            playerMovement.hardSpawn();
        }
    }
}
