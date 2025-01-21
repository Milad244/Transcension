using UnityEngine;

public class PausePanelController : MonoBehaviour
{
    private GlobalSceneManager globalSceneManager;
    [SerializeField] private GameObject pauseMenu;
    private bool paused = false;

    private void Awake()
    {
        globalSceneManager = GameObject.Find("GlobalManager").GetComponent<GlobalSceneManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        paused = !paused;

        if (paused)
        {
            globalSceneManager.pauseScene();
        }
        else
        {
            globalSceneManager.resumeScene();
        }

        pauseMenu.SetActive(paused);
    }

    public void returnToMenu()
    {
        TogglePause();
        globalSceneManager.enterMenu();
    }

    public void quitGame()
    {
        TogglePause();
        Application.Quit();
    }
}
