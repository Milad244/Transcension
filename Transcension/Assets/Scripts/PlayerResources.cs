using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private bool canTranscend;
    private GameObject transcendLevel;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (canTranscend && Input.GetKeyDown(KeyCode.F))
        {
            playerMovement.transcend(transcendLevel);
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Spikes"))
            die();

        if (col.CompareTag("Transcend"))
        {
            canTranscend = true;
            transcendLevel = col.gameObject;
        } 

    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Transcend"))
            canTranscend = false;
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
