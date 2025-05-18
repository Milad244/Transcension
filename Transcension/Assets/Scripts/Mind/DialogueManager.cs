using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextAsset jsonDialogue;
    [SerializeField] private GameObject choicePage; // active when choice given
    [SerializeField] private GameObject noChoicePage; // active when no choice given
    [SerializeField] private TextMeshProUGUI playerNoChoiceDia; // for when no choice given
    [SerializeField] private TextMeshProUGUI playerChoice1Dia; // choice 1
    [SerializeField] private TextMeshProUGUI playerChoice2Dia; // choice 2
    [SerializeField] private TextMeshProUGUI mindDia;
    [SerializeField] private GameObject tip;
    [SerializeField] private GameObject lightBackground;
    [SerializeField] private TextMeshProUGUI lightDiaText;
    [SerializeField] private GameObject finalPage;
    [SerializeField] private TextMeshProUGUI finalText;

    private DialogueContainer dialogueData;
    private Dictionary<int, DialogueNode> dialogueNodes;
    private int currentDialogueId = 1;
    private int playerOptionIndex = 0;
    private bool choice;
    private GlobalSceneManager globalSceneManager;
    private float diaWriteCD = 0.02f;
    private Coroutine dialogueCoroutine;
    private bool lightDia = false;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float lineDelay = 3f;
    private bool endStarted = false;
    private Coroutine writingDiaCoroutine;

    public void Awake()
    {
        globalSceneManager = GameObject.Find("GlobalManager").GetComponent<GlobalSceneManager>();
        resetDialogue();
        loadDialogue(globalSceneManager.diaLevel);
        displayDialogue(currentDialogueId);
        loadTip(globalSceneManager.diaLevel);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerOptionIndex = 0;
            selectOption();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && choice)
        {
            playerOptionIndex = 1;
            selectOption();
        }
    }

    private void loadTip(string key)
    {
        if (key == "intro")
        {
            tip.SetActive(true);
        }
        if (key == "tran5")
        {
            lightDia = true;
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
            return;
        }

        DialogueNode node = dialogueNodes[id];

        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);  // Stop any previous coroutine
        }
        if (writingDiaCoroutine != null) // Not allowed to continue current dialogue is finished
            return;

        writingDiaCoroutine = StartCoroutine(writeDialogue(node.dialogue));

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

    private IEnumerator writeDialogue(string dialogue)
    {
        string dialogueWriting = "";
        foreach (char character in dialogue)
        {
            dialogueWriting += character;
            mindDia.SetText(dialogueWriting);
            yield return new WaitForSeconds(diaWriteCD);
        }
        writingDiaCoroutine = null;
    }

    public void writeNext(string dialogueWriting)
    {
        mindDia.SetText(dialogueWriting);
    }

    private void selectOption()
    {
        if (writingDiaCoroutine != null) // Not allowed to continue current dialogue is finished
            return;

        if (!dialogueNodes.ContainsKey(currentDialogueId))
        {
            return;
        }

        DialogueNode node = dialogueNodes[currentDialogueId];

        if (playerOptionIndex < 0 || playerOptionIndex >= node.options.Count)
        {
            return;
        }

        int nextId = node.options[playerOptionIndex].nextId;
        if (nextId == -1 && !endStarted)
        {
            endStarted = true;
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
        lightDiaText.SetText("");
    }

    private void endMind()
    {
        if (GlobalSceneManager.Instance != null)
        {
            if (!lightDia)
                GlobalSceneManager.Instance.continueGame();

            playLightDia();
        }
        else
        {
            Debug.LogError("GlobalSceneManager instance not found!");
        }
    }

    private void playLightDia()
    {
        resetDialogue();
        StartCoroutine(bringLight());
    }

    IEnumerator bringLight()
    {
        lightBackground.SetActive(true);
        Image lightImage = lightBackground.GetComponent<Image>();
        float timer = 0f;
        Color c = lightImage.color;
        float fadeTime = 2f;
        while (timer <= fadeTime)
        {
            c.a = Mathf.Lerp(0f, 1f, timer / fadeTime);
            lightImage.color = c;
            timer += Time.deltaTime;
            yield return null;
        }
        c.a = 1f;
        lightImage.color = c;
        yield return new WaitForSeconds(1f);
        StartCoroutine(writeLightDia());
    }

    IEnumerator writeLightDia()
    {
        string[] lines = new string[]
        {
            "What is peace without freedom?",
            "Success without pain?",
            "Life without failure?",
            "A prison of the mind, with the guard of a self.",
            "But as I break free, I invite you with me.",
            "To come into the light"
        };

        lightDiaText.SetText("");

        foreach (string line in lines)
        {
            yield return StartCoroutine(FadeOutText());
            lightDiaText.SetText(line + "\n");
            yield return StartCoroutine(FadeInText());
            yield return new WaitForSeconds(lineDelay);
        }

        // final faid out
        yield return StartCoroutine(FadeOutText());
        finalPage.SetActive(true);
        finalText.SetText("Total deaths: " + globalSceneManager.deathCount);
        globalSceneManager.finishSave();
    }

    public void finishBtn()
    {
        globalSceneManager.enterMenu();
    }

    IEnumerator FadeInText()
    {
        float timer = 0f;
        Color c = lightDiaText.color;
        while (timer <= fadeDuration)
        {
            c.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            lightDiaText.color = c;
            timer += Time.deltaTime;
            yield return null;
        }
        c.a = 1f;
        lightDiaText.color = c;
    }

    IEnumerator FadeOutText()
    {
        float timer = 0f;
        Color c = lightDiaText.color;
        while (timer <= fadeDuration)
        {
            c.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            lightDiaText.color = c;
            timer += Time.deltaTime;
            yield return null;
        }
        c.a = 0f;
        lightDiaText.color = c;
    }
}
