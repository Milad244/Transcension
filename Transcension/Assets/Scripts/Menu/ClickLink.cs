using System;
using UnityEngine;

public class ClickLink : MonoBehaviour
{
    [SerializeField] private string link;

    public void openLink()
    {
        Application.OpenURL(link);
    }
}
