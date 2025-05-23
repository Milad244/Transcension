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

    public void openPlayPage()
    {
        closeAll();
        playPage.SetActive(true);
        handleContinue();
    }

    private void handleContinue()
    {
        if (globalSceneManager.level == 0)
        {
            continueBtn.interactable = false;
        }
    }

    public void openCredits()
    {
        closeAll();
        creditsPage.SetActive(true);
    }

    public void openSettings()
    {
        closeAll();
        settingsPage.SetActive(true);
        loadPrefs();
    }

    private void loadPrefs()
    {
        Dictionary<GlobalSceneManager.Binds, KeyCode> d = globalSceneManager.keyBinds;

        jumpText.SetText(globalSceneManager.cleanKeyCode(d[GlobalSceneManager.Binds.Jump]));
        tranText.SetText(globalSceneManager.cleanKeyCode(d[GlobalSceneManager.Binds.Tran]));
        choice1Text.SetText(globalSceneManager.cleanKeyCode(d[GlobalSceneManager.Binds.Choice1]));
        choice2Text.SetText(globalSceneManager.cleanKeyCode(d[GlobalSceneManager.Binds.Choice2]));
    }

    public void rebind(int bindIndex)
    {
        if (bindCoroutine != null)
        {
            StopCoroutine(bindCoroutine);
        }
        StartCoroutine(getNextKey(bindIndex));
    }

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

    public void quitGame()
    {
        globalSceneManager.quitGame();
    }

    public void openMain()
    {
        closeAll();
        mainPage.SetActive(true);
    }

    public void continueGame()
    {
        globalSceneManager.continueGame();
    }

    public void playNewGame()
    {
        globalSceneManager.startNewGame();
    }
}
