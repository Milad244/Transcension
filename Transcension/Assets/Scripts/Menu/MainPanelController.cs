using UnityEngine;

public class MainPanelController : MonoBehaviour
{

    private GlobalSceneManager globalSceneManager;
    [SerializeField] private GameObject mainPage;
    [SerializeField] private GameObject playPage;
    [SerializeField] private GameObject settingsPage;
    [SerializeField] private GameObject creditsPage;

    private void Awake()
    {
        globalSceneManager = GameObject.Find("GlobalManager").GetComponent<GlobalSceneManager>();
    }
    private void closeAll()
    {
        mainPage.SetActive(false);
        playPage.SetActive(false);
        settingsPage.SetActive(false);
        creditsPage.SetActive(false);
    }

    public void openPlayPage()
    {
        closeAll();
        playPage.SetActive(true);
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

    public void playNewGame()
    {
        globalSceneManager.startNewGame();
    }
}
