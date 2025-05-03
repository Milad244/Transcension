using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    [SerializeField] private GameObject minePrefab;
    private PlayerMovement playerMovement;
    private bool canTranscend;
    private GameObject transcendLevel;
    private List<GameObject> deactiveMineTriggers;
    private GlobalSceneManager globalSceneManager;
    private UIControl uiControl;
    private List<Head> activeHeads;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        deactiveMineTriggers = new List<GameObject>();
        activeHeads = new List<Head>();
        globalSceneManager = GameObject.Find("GlobalManager").GetComponent<GlobalSceneManager>();
        uiControl = GameObject.Find("Canvas UI").GetComponent<UIControl>();
    }

    private void Update()
    {
        if (globalSceneManager.isBlocked)
            return;

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
            uiControl.transcendTipActive(true);
            canTranscend = true;
            transcendLevel = colGameObj;
        } 

        if (col.CompareTag("MTrigger"))
        {
            setMine(colGameObj.transform.position);
            deactiveMineTriggers.Add(colGameObj);
            colGameObj.SetActive(false);
        }

        if (col.CompareTag("HeadDetect")) {
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
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Transcend"))
        {
            uiControl.transcendTipActive(false);
            canTranscend = false;
        }
    }

    public void die()
    {
        playerMovement.dieMovement();
    }

    private void revive() //Called in animation
    {
        playerMovement.reviveMovement();

        if (deactiveMineTriggers != null)
        {
            foreach (GameObject deactiveMineTrigger in deactiveMineTriggers) 
            {
                deactiveMineTrigger.SetActive(true);
            }
            deactiveMineTriggers.Clear();
        }
        
        if (activeHeads != null) {
            foreach (Head head in activeHeads) 
            {
                head.GetComponent<Head>().startDeactivate();
            }
            activeHeads.Clear();
        }
    }

    private void setMine(Vector3 tPosition)
    {
        Vector3 mPosition = new Vector3(tPosition.x, tPosition.y + 10, tPosition.z);
        Instantiate(minePrefab, mPosition, Quaternion.identity);
    }
}
