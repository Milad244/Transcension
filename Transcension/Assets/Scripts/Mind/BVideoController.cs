using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class BVideoController : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    void Awake()
    {
        // Gets the mind background loop video and plays it.
        string videoPath = Path.Combine(Application.streamingAssetsPath, "BLoop.mp4");
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoPath;
        videoPlayer.Play();
    }
}
