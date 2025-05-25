using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

    public enum Binds
    {
        Jump, Tran, Choice1, Choice2
    }
    public Dictionary<Binds, string> bindPrefs = new Dictionary<Binds, string>
    {
        {Binds.Jump, "JumpPref"},
        {Binds.Tran, "TranPref"},
        {Binds.Choice1, "Choice1Pref"},
        {Binds.Choice2, "Choice2Pref"}
    };
    public Dictionary<Binds, KeyCode> keyBinds = new Dictionary<Binds, KeyCode> { };
    public Dictionary<Binds, KeyCode> defaultKeyBinds = new Dictionary<Binds, KeyCode>
    {
        {Binds.Jump, KeyCode.Space},
        {Binds.Tran, KeyCode.F},
        {Binds.Choice1, KeyCode.Alpha1},
        {Binds.Choice2, KeyCode.Alpha2}
    };

    /// <summary>
    /// Goes through all the playerPref keys and sets the keybind for each.
    /// If a playerPref exists, it uses that key. Otherwise, it assigns the default key.
    /// </summary>
    public void getBinds()
    {
        foreach (KeyValuePair<Binds, string> entry in bindPrefs)
        {
            if (PlayerPrefs.HasKey(entry.Value))
            {
                string keyString = PlayerPrefs.GetString(entry.Value);
                KeyCode keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), keyString);
                keyBinds[entry.Key] = keyCode;
            }
            else
            {
                setBind(entry.Key, defaultKeyBinds[entry.Key]);
            }
        }
    }

    /// <summary>
    /// Sets a new key to a bind and saves it in playerPrefs.
    /// </summary>
    /// <param name="bind">The bind to set.</param>
    /// <param name="newKey">The new key of the bind.</param>
    public void setBind(Binds bind, KeyCode newKey)
    {
        keyBinds[bind] = newKey;
        PlayerPrefs.SetString(bindPrefs[bind], newKey.ToString());
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Gets a cleaner string to represent a given key for the user.
    /// </summary>
    /// <param name="keyCode">The key to clean.</param>
    /// <returns>The cleaned key.</returns>
    public string cleanKeyCode(KeyCode keyCode)
    {
        string keyString = keyCode.ToString();
        return Regex.Replace(keyString, @"Alpha(\d)", "$1");
    }

    /// <summary>
    /// Sets the new level of the game and saves it in playerPrefs.
    /// </summary>
    /// <param name="level_">The new level of the game.</param>
    public void setLevel(int level_)
    {
        level = level_;
        PlayerPrefs.SetInt(levelPref, level);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Sets the new death count and saves it in playerPrefs.
    /// </summary>
    /// <param name="deathCount_">The new death count.</param>
    private void setDeathCount(int deathCount_)
    {
        deathCount = deathCount_;
        PlayerPrefs.SetInt(deathCountPref, deathCount);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Increments the death count and saves it in playerPrefs.
    /// </summary>
    public void incrementDeathCount()
    {
        deathCount++;
        PlayerPrefs.SetInt(deathCountPref, deathCount);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Gets the level from playerPrefs if it exists. Otherwise, sets the level at 0.
    /// </summary>
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

    /// <summary>
    /// Gets the death count from playerPrefs if it exists. Otherwise, sets the death count at 0.
    /// </summary>
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
        // If an instance of globalSceneManager doesn't exist, make this one that instance. Otherwise, destory this duplicate instance.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            getLevelForAwake();
            getDeathForAwake();
            getBinds();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Resets the level and death count to 0 because the save is finished (user finished the game).
    /// </summary>
    public void finishSave()
    {
        setLevel(0);
        setDeathCount(0);
    }

    /// <summary>
    /// Starts a new game with the level and death count set to 0. Loads the game scene and turns off the cursor.
    /// </summary>
    public void startNewGame()
    {
        setLevel(0);
        setDeathCount(0);
        SceneManager.LoadScene((int)SceneName.Game);
        Cursor.visible = false;
    }

    /// <summary>
    /// Continues the user's existing save. Loads the game scene and turns off the cursor.
    /// </summary>
    public void continueGame()
    {
        SceneManager.LoadScene((int)SceneName.Game);
        Cursor.visible = false;
    }

    /// <summary>
    /// Loads the menu scene and turns on the cursor.
    /// </summary>
    public void enterMenu()
    {
        Cursor.visible = true;
        SceneManager.LoadScene((int)SceneName.Menu);
    }

    /// <summary>
    /// Loads the mind scene with a given dialogue level.
    /// </summary>
    /// <param name="diaLevel">The dialogue level.</param>
    public void loadMindScene(string diaLevel)
    {
        this.diaLevel = diaLevel;
        SceneManager.LoadScene((int)SceneName.Mind);
    }

    /// <summary>
    /// Turns on the cursor, stops time, and enables the isBlocked boolean flag.
    /// </summary>
    public void pauseScene()
    {
        Cursor.visible = true;
        Time.timeScale = 0f;
        isBlocked = true;
    }

    /// <summary>
    /// Turns off the cursor, unstops time, and calls resumeWithDelay().
    /// </summary>
    public void resumeScene()
    {
        Cursor.visible = false;
        Time.timeScale = 1f;
        StartCoroutine(resumeWithDelay());
    }

    /// <summary>
    /// Disables the isBlocked boolean flag after a set resume delay passes.
    /// </summary>
    /// <returns></returns>
    private IEnumerator resumeWithDelay()
    {
        yield return new WaitForSeconds(resumeDelay);
        isBlocked = false; // Allow input after the delay
    }

    /// <summary>
    /// Safely quits the game.
    /// </summary>
    public void quitGame()
    {
        StartCoroutine(safeQuit());
    }

    /// <summary>
    /// Waits for the last frame and quits the application. This way we avoid other code interfering with our quitting - which can lead to crashes.
    /// </summary>
    private IEnumerator safeQuit()
    {
        yield return new WaitForEndOfFrame();
        Application.Quit();
    }
}
