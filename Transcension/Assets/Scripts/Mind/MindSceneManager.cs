using UnityEngine;

public class MindSceneManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GlobalSceneManager.Instance != null)
            {
                GlobalSceneManager.Instance.ExitMindScene("Mind");
            }
            else
            {
                Debug.LogError("GlobalSceneManager instance not found!");
            }
        }
    }
}
