using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainPanelController : MonoBehaviour
{

    private GlobalSceneManager globalSceneManager;
    [SerializeField] private GameObject mainPage;
    [SerializeField] private GameObject playPage;
    [SerializeField] private GameObject settingsPage;
    [SerializeField] private GameObject creditsPage;
    [SerializeField] private Button continueBtn;

    [SerializeField] private GameObject bindPage;
    [SerializeField] private TextMeshProUGUI jumpText;
    [SerializeField] private TextMeshProUGUI tranText;
    [SerializeField] private TextMeshProUGUI choice1Text;
    [SerializeField] private TextMeshProUGUI choice2Text;
    private Coroutine bindCoroutine;

    private void Awake()
    {
        globalSceneManager = GameObject.Find("GlobalManager").GetComponent<GlobalSceneManager>();
    }

    /// <summary>
    /// Stops the bind coroutine and closes all the menu pages.
    /// </summary>
    private void closeAll()
    {
        bindPage.SetActive(false);
        if (bindCoroutine != null)
        {
            StopCoroutine(bindCoroutine);
        }

        mainPage.SetActive(false);
        playPage.SetActive(false);
        settingsPage.SetActive(false);
        creditsPage.SetActive(false);
    }

    /// <summary>
    /// Opens the play page where the user can choose to play.
    /// </summary>
    public void openPlayPage()
    {
        closeAll();
        playPage.SetActive(true);
        handleContinue();
    }

    /// <summary>
    /// Disables the continue button if the save level is 0.
    /// </summary>
    private void handleContinue()
    {
        if (globalSceneManager.level == 0)
        {
            continueBtn.interactable = false;
        }
    }

    /// <summary>
    /// Opens the credits page where the user can see my name and my project's GitHub.
    /// </summary>
    public void openCredits()
    {
        closeAll();
        creditsPage.SetActive(true);
    }

    /// <summary>
    /// Opens the settings page where the user can change some keybinds.
    /// </summary>
    public void openSettings()
    {
        closeAll();
        settingsPage.SetActive(true);
        loadPrefs();
    }

    /// <summary>
    /// Loads the current keybinds and displays them to the user.
    /// </summary>
    private void loadPrefs()
    {
        Dictionary<GlobalSceneManager.Binds, KeyCode> d = globalSceneManager.keyBinds;

        jumpText.SetText(globalSceneManager.cleanKeyCode(d[GlobalSceneManager.Binds.Jump]));
        tranText.SetText(globalSceneManager.cleanKeyCode(d[GlobalSceneManager.Binds.Tran]));
        choice1Text.SetText(globalSceneManager.cleanKeyCode(d[GlobalSceneManager.Binds.Choice1]));
        choice2Text.SetText(globalSceneManager.cleanKeyCode(d[GlobalSceneManager.Binds.Choice2]));
    }

    /// <summary>
    /// Starts the set bind coroutine with an index that represents the bind to change.
    /// </summary>
    /// <param name="bindIndex">The index of the bind to change.</param>
    public void rebind(int bindIndex)
    {
        if (bindCoroutine != null)
        {
            StopCoroutine(bindCoroutine);
        }
        StartCoroutine(getNextKey(bindIndex));
    }

    /// <summary>
    /// Activates the pick bind page. 
    /// Waits for the user to press a key to rebind the bind represented by the given bind index. 
    /// Then, closes the pick bind page and reloads all the binds.
    /// </summary>
    /// <param name="bindIndex">The index of the bind to change.</param>
    private IEnumerator getNextKey(int bindIndex)
    {
        GlobalSceneManager.Binds bind = (GlobalSceneManager.Binds)bindIndex;
        bindPage.SetActive(true);

        bool keyPressed = false;

        while (!keyPressed)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    globalSceneManager.setBind(bind, keyCode);
                    keyPressed = true;
                    bindPage.SetActive(false);
                    loadPrefs();
                    break;
                }
            }
            yield return null;
        }
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void quitGame()
    {
        globalSceneManager.quitGame();
    }

    /// <summary>
    /// Opens the main page where the user can select other pages.
    /// </summary>
    public void openMain()
    {
        closeAll();
        mainPage.SetActive(true);
    }

    /// <summary>
    /// Continues the current save.
    /// </summary>
    public void continueGame()
    {
        globalSceneManager.continueGame();
    }

    /// <summary>
    /// Starts a new save.
    /// </summary>
    public void playNewGame()
    {
        globalSceneManager.startNewGame();
    }
}
