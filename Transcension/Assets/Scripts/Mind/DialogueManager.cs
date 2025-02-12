using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections;

public class DialogueManager : MonoBehaviour // this entire script is not even close to functional btw.
{
    [SerializeField] private TextAsset jsonDialogue;
    [SerializeField] private GameObject choicePage; // active when choice given
    [SerializeField] private GameObject noChoicePage; // active when no choice given
    [SerializeField] private GameObject choice1Page;
     [SerializeField] private GameObject choice2Page;
    [SerializeField] private TextMeshProUGUI playerNoChoiceDia; // for when no choice given
    [SerializeField] private TextMeshProUGUI playerChoice1Dia; // choice 1
    [SerializeField] private TextMeshProUGUI playerChoice2Dia; // choice 2
    [SerializeField] private TextMeshProUGUI mindDia;

    private DialogueContainer dialogueData;
    private Dictionary<int, DialogueNode> dialogueNodes;
    private int currentDialogueId = 1;
    private int playerOptionIndex = 0;
    private bool choice;
    private GlobalSceneManager globalSceneManager;
    private float diaWriteCD = 0.5f;
    private Coroutine dialogueCoroutine;

    public void Awake()
    {
        globalSceneManager = GameObject.Find("GlobalManager").GetComponent<GlobalSceneManager>();
        resetDialogue();
        loadDialogue(globalSceneManager.diaLevel);
        displayDialogue(currentDialogueId);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // ToDo: make sure you can't continue until dialogue is finished writing
            selectOption();
        }

        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)) && choice){
            if (playerOptionIndex == 0)
            {
                changeUserChoice(1);
            } else{
                changeUserChoice(0);
            }
        }
    }

    private void loadDialogue(string key)
    {
        if (jsonDialogue == null)
        {
            Debug.LogError("No JSON file assigned to DialogueManager!");
            return;
        }

        dialogueData = JsonUtility.FromJson<DialogueContainer>(jsonDialogue.text);

        if (dialogueData == null || dialogueData.Dialogue == null)
        {
            Debug.LogError("Failed to load dialogue from JSON.");
            return;
        }

        DialogueTree selectedTree = dialogueData.Dialogue.FirstOrDefault(tree => tree.key == key);

        if (selectedTree == null)
        {
            Debug.LogError($"Dialogue key '{key}' not found!");
            return;
        }

        dialogueNodes = new Dictionary<int, DialogueNode>();
        foreach (var node in selectedTree.dialogueTree)
        {
            dialogueNodes[node.id] = node;
        }
    }

    private void displayDialogue(int id)
    {
        if (!dialogueNodes.ContainsKey(id))
        {
            Debug.LogError($"Dialogue ID {id} not found!");
            return;
        }

        DialogueNode node = dialogueNodes[id];

        if (dialogueCoroutine != null)
        {
            Debug.Log("This does not ever print");
            StopCoroutine(dialogueCoroutine);  // Stop any previous coroutine
        }

        dialogueCoroutine = StartCoroutine(writeDialogue());//node.dialogue

        if (node.options.Count == 1)
        {
            choice = false;
            playerOptionIndex = 0;
            choicePage.SetActive(false);
            noChoicePage.SetActive(true);
            playerNoChoiceDia.SetText(node.options[0].text);
        }
        else // only two options
        {
            choice = true;
            choicePage.SetActive(true);
            noChoicePage.SetActive(false);
            playerChoice1Dia.SetText(node.options[0].text);
            playerChoice2Dia.SetText(node.options[1].text);
        }
    }

    IEnumerator writeDialogue()//string dialogue
    {
        // string dialogueWriting = "";
        // string test = "Test";
        // Debug.Log(dialogue);
        // foreach(char character in test){
        //     Debug.Log(character);
        //     dialogueWriting += character;
        //     mindDia.SetText(dialogueWriting);
        //     //yield return new WaitForSeconds(diaWriteCD);
        //     Debug.Log(character);
        // }
        Debug.Log("Starting");
        yield return new WaitForSeconds(1);
        Debug.Log("Finished");
    }

    public void changeUserChoice(int newPlayerOptionIndex)
    {
        playerOptionIndex = newPlayerOptionIndex;

        if (newPlayerOptionIndex == 0){
            choice1Page.GetComponent<Image>().color = ColorUtility.TryParseHtmlString("#4D82A4", out var color1) ? color1 : Color.blue;
            choice2Page.GetComponent<Image>().color = ColorUtility.TryParseHtmlString("#898989", out var color2) ? color2 : Color.white;
        } else{
            choice1Page.GetComponent<Image>().color = ColorUtility.TryParseHtmlString("#898989", out var color2) ? color2 : Color.white;
            choice2Page.GetComponent<Image>().color = ColorUtility.TryParseHtmlString("#4D82A4", out var color1) ? color1 : Color.blue;
        }
    } 

    private void selectOption()
    {
        if (!dialogueNodes.ContainsKey(currentDialogueId))
        {
            Debug.LogError($"Invalid dialogue ID: {currentDialogueId}");
            return;
        }

        DialogueNode node = dialogueNodes[currentDialogueId];

        if (playerOptionIndex < 0 || playerOptionIndex >= node.options.Count)
        {
            Debug.LogError("Invalid option selected!");
            return;
        }

        int nextId = node.options[playerOptionIndex].nextId;
        if (nextId == -1)
        {
            endMind();
            return;
        }

        currentDialogueId = nextId;
        displayDialogue(currentDialogueId);
    }

    private void resetDialogue()
    {
        choicePage.SetActive(false);
        noChoicePage.SetActive(false);
        playerNoChoiceDia.SetText("");
        playerChoice1Dia.SetText("");
        playerChoice2Dia.SetText("");
        mindDia.SetText("");
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
}
