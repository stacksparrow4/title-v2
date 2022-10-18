using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class cutScene : MonoBehaviour
{
    // This didn't end up being used because I didn't have time to add a cutscene, but the logic is still there


    private VideoPlayer videoPlayer;
    private bool hasPlayed;

    [SerializeField] Text skipCutsceneText;
    private bool spacePressedOnce;
    
    private void Start()
    {
        //Assigns the video player to the component, then immediately begins playing the video
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.Play();
    }
    private void Update()
    {
        SkipCutscene();
        LoadNextScene();
    }


    private void SkipCutscene()
    {
        //If space is pressed, the cutscene gets skipped and the next scene is loaded
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void LoadNextScene()
    {
        //This line was to stop the loading of the next scene in the few frames before the video started playing
        //It ensures that the cutscene will play at least once and then load the next scene
        if (videoPlayer.isPlaying)
        { hasPlayed = true; }

        if (!videoPlayer.isPlaying && hasPlayed)
        { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); }
    }
}
