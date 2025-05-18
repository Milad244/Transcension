using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalSceneManager : MonoBehaviour
{
    public static GlobalSceneManager Instance { get; private set; }

    public bool isBlocked = false; // For any blocking of actions
    public int level = 0;
    public int deathCount = 0;
    private float resumeDelay = 0.1f;
    public string diaLevel { get; private set; }
    public enum SceneName
    {
        Menu = 0,
        Game = 1,
        Mind = 2
    }
    public string levelPref { get; private set; } = "LevelPref";
    public string deathCountPref { get; private set; } = "DeathCountPref";

    public void setLevel(int level_)
    {
        level = level_;
        PlayerPrefs.SetInt(levelPref, level);
        PlayerPrefs.Save();
    }
    private void setDeathCount(int deathCount_)
    {
        deathCount = deathCount_;
        PlayerPrefs.SetInt(deathCountPref, deathCount);
        PlayerPrefs.Save();
    }
    public void addToDeathCount()
    {
        deathCount++;
        PlayerPrefs.SetInt(deathCountPref, deathCount);
        PlayerPrefs.Save();
    }

    private void getLevelForAwake()
    {
        if (PlayerPrefs.HasKey(levelPref))
        {
            level = PlayerPrefs.GetInt(levelPref);
        }
        else
        {
            setLevel(0);
        }
    }

    private void getDeathForAwake()
    {
        if (PlayerPrefs.HasKey(deathCountPref))
        {
            deathCount = PlayerPrefs.GetInt(deathCountPref);
        }
        else
        {
            setDeathCount(0);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            getLevelForAwake();
            getDeathForAwake();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void finishSave()
    {
        setLevel(0);
        setDeathCount(0);
    }

    public void startNewGame()
    {
        setLevel(0);
        setDeathCount(0);
        SceneManager.LoadScene((int)SceneName.Game);
    }

    public void continueGame()
    {
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
