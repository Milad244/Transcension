using UnityEngine;

public class ClickLink : MonoBehaviour
{
    [SerializeField] private string link;

    /// <summary>
    /// Opens a set link.
    /// </summary>
    public void openLink()
    {
        Application.OpenURL(link);
    }
}
