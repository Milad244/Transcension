using UnityEngine;

public class MindSceneManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (GlobalSceneManager.Instance != null)
            {
                GlobalSceneManager.Instance.unloadMindScene();
            }
            else
            {
                Debug.LogError("GlobalSceneManager instance not found!");
            }
        }
    }
}
