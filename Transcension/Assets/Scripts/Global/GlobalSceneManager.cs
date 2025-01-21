using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalSceneManager : MonoBehaviour
{
    public static GlobalSceneManager Instance { get; private set; }

    private bool isPaused = false; // For in-scene pause
    private bool isAbsPaused = false; // For outside-scene pause
    public bool isBlocked = false; // For any blocking of actions
    public string gameDifficulty { get; private set; } = "easy"; // Default difficulty incase bugs
    private float resumeDelay = 0.1f;

    public enum SceneName
    {
        Menu = 0,
        Game = 1,
        Mind = 2
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void startNewGame(string difficulty)
    {
        gameDifficulty = difficulty;
        SceneManager.LoadScene((int)SceneName.Game);
    }

    public void continueGame()
    {
        // TBD
        SceneManager.LoadScene((int)SceneName.Game);
    }

    public void enterMenu()
    {
        SceneManager.LoadScene((int)SceneName.Menu);
    }

    public void loadMindScene()
    {
        if (!isPaused && !isAbsPaused)
        {
            absPause();
            SceneManager.LoadScene((int)SceneName.Mind, LoadSceneMode.Additive);
        }
    }

    public void unloadMindScene()
    {
        if (isAbsPaused)
        {
            SceneManager.UnloadSceneAsync((int)SceneName.Mind);
            resumeAbsPause();
        }
    }

    public void pauseScene()
    {
        Time.timeScale = 0f;
        isPaused = true;
        isBlocked = true;
    }

    public void resumeScene()
    {
        Time.timeScale = 1f;
        isPaused = false;
        StartCoroutine(resumeWithDelay());
    }

    public void absPause()
    {
        Time.timeScale = 0f;
        toggleRootGameObjects(false);
        isAbsPaused = true;
        isBlocked = true;
    }

    public void resumeAbsPause()
    {
        Time.timeScale = 1f;
        toggleRootGameObjects(true);
        isAbsPaused = false;
        StartCoroutine(resumeWithDelay());
    }

    // Coroutine to delay resumption of inputs
    private IEnumerator resumeWithDelay()
    {
        yield return new WaitForSeconds(resumeDelay);
        isBlocked = false; // Allow input after the delay
    }

    // Helper method to enable/disable root game objects
    private void toggleRootGameObjects(bool isActive)
    {
        foreach (GameObject obj in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            obj.SetActive(isActive);
        }
    }
}
