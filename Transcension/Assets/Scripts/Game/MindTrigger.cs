using System;
using UnityEngine;

public class MindTrigger : MonoBehaviour
{
    [SerializeField] private string mindLevel;
    private GlobalSceneManager globalSceneManager;

    private void Awake()
    {
        globalSceneManager = GameObject.Find("GlobalManager").GetComponent<GlobalSceneManager>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            globalSceneManager.loadMindScene(mindLevel);
            Destroy(gameObject);
        }
    }
}
