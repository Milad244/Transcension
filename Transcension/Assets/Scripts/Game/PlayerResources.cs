using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    [SerializeField] private GameObject minePrefab;
    private PlayerMovement playerMovement;
    private bool canTranscend;
    private GameObject transcendLevel;
    private List<GameObject> deactiveMineTriggers;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        deactiveMineTriggers = new List<GameObject>();
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
        GameObject colGameObj = col.gameObject;
        if (col.CompareTag("Spikes"))
            die();

        if (col.CompareTag("Transcend"))
        {
            canTranscend = true;
            transcendLevel = colGameObj;
        } 

        if (col.CompareTag("MTrigger"))
        {
            setMine(colGameObj.transform.position);
            deactiveMineTriggers.Add(colGameObj);
            colGameObj.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Transcend"))
            canTranscend = false;
    }

    public void die()
    {
        playerMovement.dieMovement();
    }

    private void revive() //Called in animation
    {
        playerMovement.reviveMovement();

        foreach (GameObject deactiveMineTrigger in deactiveMineTriggers) {
            deactiveMineTrigger.SetActive(true);
        } 
    }

    private void setMine(Vector3 tPosition)
    {
        Vector3 mPosition = new Vector3(tPosition.x, tPosition.y + 10, tPosition.z);
        Instantiate(minePrefab, mPosition, Quaternion.identity);
    }
}
