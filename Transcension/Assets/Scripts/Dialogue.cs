using System;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private string dialogueName;

    private void OnTriggerEnter2D(Collider2D col)
    {
        dialogueManager.playDialogue(dialogueName);
        Destroy(gameObject);
    }
}
