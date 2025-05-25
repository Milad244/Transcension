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

    /// <summary>
    /// Toggles the game's paused state and pauses or unpauses the game accordingly.
    /// </summary>
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

    /// <summary>
    /// Returns the user to the menu scene.
    /// </summary>
    public void returnToMenu()
    {
        TogglePause();
        globalSceneManager.enterMenu();
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void quitGame()
    {
        globalSceneManager.quitGame();
    }
}
