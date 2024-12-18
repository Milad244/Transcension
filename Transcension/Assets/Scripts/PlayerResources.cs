using Unity.VisualScripting;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Spikes"))
            die();
    }

    private void die()
    {
        playerMovement.dieMovement();
    }

    private void revive()
    {
        playerMovement.reviveMovement();
    }
}
