using UnityEngine;
using UnityEngine.Video;

public class BVideoController : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    void Awake()
    {
        videoPlayer.Play();
    }
}
