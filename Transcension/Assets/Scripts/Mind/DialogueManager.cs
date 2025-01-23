using System;
using System.Collections.Generic;
using TMPro;
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

public class DialogueManager : MonoBehaviour // this entire script is not even close to functional btw.
{
    private Dictionary<string, List<string>> dialogues;
    [SerializeField] private TextAsset jsonDialogue;
    [SerializeField] private TextMeshProUGUI playerDia1; //choice 1
    [SerializeField] private TextMeshProUGUI playerDia2; // choice 2
    [SerializeField] private TextMeshProUGUI mindDia;
    private int currentLineIndex = 0;

    public void Awake()
    {
        //playerDia1.SetText("");
        //playerDia2.SetText("");
        //mindDia.SetText("");

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("endMind?");
            endMind(); // Will add next dialogue here later NextLine();
        }
    }

    private void endMind()
    {
        if (GlobalSceneManager.Instance != null)
        {
            GlobalSceneManager.Instance.unloadMindScene();
        }
        else
        {
            Debug.LogError("GlobalSceneManager instance not found!");
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

    public void NextLine(string dialogueName)
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
                endMind();
            }
        }
    }

    private void DisplayCurrentLine(List<string> dialogueLines)
    {
        string currentLine = dialogueLines[currentLineIndex];
        //uiControl.writeDialogue(currentLine);
    }
}
