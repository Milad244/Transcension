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
            Debug.Log($"Scene to mind level {mindLevel}"); //Adding later
            globalSceneManager.EnterMindScene("Mind");
            Destroy(gameObject);
        }
    }
}
