using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class video_scene : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string sceneName;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene("Start Screen");
    }
}