using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueEntry
{
    public string key;
    public List<string> lines;
}

[Serializable]
public class DialogueData
{
    public List<DialogueEntry> Dialogue;
}

public class DialogueManager : MonoBehaviour
{
    private Dictionary<string, List<string>> dialogues;
    [SerializeField] private TextAsset jsonDialogue;
    //[SerializeField] private UIControl uiControl;
    private int currentLineIndex = 0;

    public void Awake()
    {
        // Parsing JSON Dialogue
        if (jsonDialogue == null)
        {
            Debug.LogError("No JSON file assigned to DialogueManager!");
            return;
        }

        DialogueData data = JsonUtility.FromJson<DialogueData>(jsonDialogue.text);
        dialogues = new Dictionary<string, List<string>>();

        foreach (var entry in data.Dialogue)
        {
            dialogues[entry.key] = entry.lines;
        }

        if (dialogues.Count == 0)
        {
            Debug.LogError("Failed to parse dialogues from the JSON file!");
        }
    }

    public void playDialogue(String dialogueName)
    {
        if (dialogues.TryGetValue(dialogueName, out List<string> dialogueLines))
        {
            currentLineIndex = 0;
            DisplayCurrentLine(dialogueLines);
        }
        else
        {
            Debug.LogWarning($"Dialogue with key '{dialogueName}' not found!");
        }
    }

    public void NextLine(string dialogueName) // Called from outside this script after player 'continues' the dialogue
    {
        if (dialogues.TryGetValue(dialogueName, out List<string> dialogueLines))
        {
            if (currentLineIndex + 1 < dialogueLines.Count)
            {
                currentLineIndex++;
                DisplayCurrentLine(dialogueLines);
            }
            else
            {
                //uiControl.stopDialogue();
            }
        }
    }

    private void DisplayCurrentLine(List<string> dialogueLines)
    {
        string currentLine = dialogueLines[currentLineIndex];
        //uiControl.writeDialogue(currentLine);
    }
}
