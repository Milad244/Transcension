using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class BVideoController : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    void Awake()
    {
        string videoPath = Path.Combine(Application.streamingAssetsPath, "BLoop.mp4");
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoPath;
        videoPlayer.Play();
    }
}
