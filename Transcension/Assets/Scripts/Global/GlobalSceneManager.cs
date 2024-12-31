using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalSceneManager : MonoBehaviour
{
    public static GlobalSceneManager Instance { get; private set; }

    private bool isPaused = false;
    private string pausedSceneName;

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

    public void EnterMindScene(string mindSceneName)
    {
        if (!isPaused)
        {
            pausedSceneName = SceneManager.GetActiveScene().name;
            PauseScene();

            SceneManager.LoadScene(mindSceneName, LoadSceneMode.Additive);
        }
    }

    public void ExitMindScene(string mindSceneName)
    {
        if (isPaused)
        {
            SceneManager.UnloadSceneAsync(mindSceneName);

            ResumeScene();
        }
    }

    private void PauseScene()
    {
        Time.timeScale = 0f;

        foreach (GameObject obj in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            obj.SetActive(false);
        }

        isPaused = true;
    }

    private void ResumeScene()
    {
        Time.timeScale = 1f;

        foreach (GameObject obj in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            obj.SetActive(true);
        }

        isPaused = false;
    }
}
