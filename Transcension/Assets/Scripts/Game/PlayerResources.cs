using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    [SerializeField] private GameObject minePrefab;
    private PlayerMovement playerMovement;
    private bool canTranscend;
    private GameObject transcendLevel;
    private List<GameObject> deactivatedMineTriggers;
    private GlobalSceneManager globalSceneManager;
    private UIControl uiControl;
    private List<Head> activeHeads;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        deactivatedMineTriggers = new List<GameObject>();
        activeHeads = new List<Head>();
        globalSceneManager = GameObject.Find("GlobalManager").GetComponent<GlobalSceneManager>();
        uiControl = GameObject.Find("Canvas UI").GetComponent<UIControl>();
    }

    private void Update()
    {
        if (globalSceneManager.isBlocked)
            return;

        if (canTranscend && Input.GetKeyDown(globalSceneManager.keyBinds[GlobalSceneManager.Binds.Tran]))
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
            uiControl.transcendTipActive(true);
            canTranscend = true;
            transcendLevel = colGameObj;
        }

        if (col.CompareTag("MTrigger"))
        {
            setMine(colGameObj.transform.position);
            deactivatedMineTriggers.Add(colGameObj);
            colGameObj.SetActive(false);
        }

        if (col.CompareTag("HeadDetect"))
        {
            HeadDetect headDetect = colGameObj.GetComponent<HeadDetect>();
            headDetect.activateHead();
            activeHeads.Add(headDetect.getParentHead());
        }

        if (col.CompareTag("Head"))
        {
            Head head = colGameObj.GetComponent<Head>();
            head.startDeactivate();
            die();
            activeHeads.Remove(head);
        }

        if (col.CompareTag("Slow"))
        {
            playerMovement.toggleSpeedSlow(true);
        }

        if (col.CompareTag("Fireball"))
        {
            die();
            StartCoroutine(playerMovement.startBoss());
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Transcend"))
        {
            uiControl.transcendTipActive(false);
            canTranscend = false;
        }

        if (col.CompareTag("Slow"))
        {
            playerMovement.toggleSpeedSlow(false);
        }
    }

    /// <summary>
    /// Kills the player.
    /// </summary>
    public void die()
    {
        playerMovement.dieMovement();
    }

    /// <summary>
    /// Revives the player and clears all the activated traps Lists.
    /// </summary>
    private void revive() //Called after dying animation finishes
    {
        playerMovement.reviveMovement();

        if (deactivatedMineTriggers != null)
        {
            foreach (GameObject deactivatedMineTrigger in deactivatedMineTriggers)
            {
                deactivatedMineTrigger.SetActive(true);
            }
            deactivatedMineTriggers.Clear();
        }

        if (activeHeads != null)
        {
            foreach (Head head in activeHeads)
            {
                head.GetComponent<Head>().startDeactivate();
            }
            activeHeads.Clear();
        }
    }

    /// <summary>
    /// Creates a mine above the given mine trigger position.
    /// </summary>
    /// <param name="tPosition">The position of the mine trigger.</param>
    private void setMine(Vector3 tPosition)
    {
        Vector3 mPosition = new Vector3(tPosition.x, tPosition.y + 10, tPosition.z);
        Instantiate(minePrefab, mPosition, Quaternion.identity);
    }
}
