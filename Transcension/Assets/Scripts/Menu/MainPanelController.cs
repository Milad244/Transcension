using UnityEngine;

public class MainPanelController : MonoBehaviour
{

    private GlobalSceneManager globalSceneManager;
    [SerializeField] private GameObject mainPage;
    [SerializeField] private GameObject playPage1;
    [SerializeField] private GameObject playPage2;
    [SerializeField] private GameObject settingsPage;
    [SerializeField] private GameObject creditsPage;

    private void Awake()
    {
        globalSceneManager = GameObject.Find("GlobalManager").GetComponent<GlobalSceneManager>();
    }
    private void closeAll()
    {
        mainPage.SetActive(false);
        playPage1.SetActive(false);
        playPage2.SetActive(false);
        settingsPage.SetActive(false);
        creditsPage.SetActive(false);
    }

    public void openPlayPage1()
    {
        closeAll();
        playPage1.SetActive(true);
    }

    public void openPlayPage2()
    {
        closeAll();
        playPage2.SetActive(true);
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
    }

    public void quitGame()
    {
        Application.Quit();
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

    public void playNewGame(string difficulty)
    {
        globalSceneManager.startNewGame(difficulty);
    }
}
