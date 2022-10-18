using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    //Controls the LEVEL music
    [SerializeField] AudioSource levelMusic;
    private void Update()
    {
        //If the scene is the main menu, or the boss fight, then turn off the music if it's playing
        if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 8)
        { 
            if(levelMusic.isPlaying)
            { levelMusic.Stop(); }
        }
        else 
        { 
            //Else, if the music isn't playing, then start playing it
            if(!levelMusic.isPlaying)
            { levelMusic.Play(); }
        }
    }
}
