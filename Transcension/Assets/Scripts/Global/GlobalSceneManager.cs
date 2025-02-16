using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalSceneManager : MonoBehaviour
{
    public static GlobalSceneManager Instance { get; private set; }

    public bool isBlocked = false; // For any blocking of actions
    public string gameDifficulty { get; private set; } = "easy"; // Default difficulty
    public int level = 0;
    private float resumeDelay = 0.1f;
    public string diaLevel { get; private set; }
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
        level = 0;
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

    public void loadMindScene(string diaLevel)
    {
        this.diaLevel = diaLevel;
        SceneManager.LoadScene((int)SceneName.Mind);
    }

    public void pauseScene()
    {
        Time.timeScale = 0f;
        isBlocked = true;
    }

    public void resumeScene()
    {
        Time.timeScale = 1f;
        StartCoroutine(resumeWithDelay());
    }

    // Coroutine to delay resumption of inputs
    private IEnumerator resumeWithDelay()
    {
        yield return new WaitForSeconds(resumeDelay);
        isBlocked = false; // Allow input after the delay
    }
}
