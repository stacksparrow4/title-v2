using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GlobalControl : MonoBehaviour
{
    //When the game is first loaded, the defaults are loaded, then after that every time the game resets itself, the settings don't reset.
    public static bool menuDefaultsChanged = false;
    

    //Saved values used to keep settings consistent over multiple scenes
    public static float savedMasterValue;
    public static float savedMusicValue;
    public static float savedInterfaceValue;
    public static float savedAmbienceValue;
    public static bool hardCoreMode = false;

    public static int currentLoadHealth = 3;

    // Code from https://www.sitepoint.com/saving-data-between-scenes-in-unity/

    //Put simply (sort of), when this object is first loaded, it checks if there is another object like it. Another "Instance" of itself
    //If there isn't another one, dont destroy this object and set this instance to "this"
    //If there is another instance, but it isn't "this" object, then destroy the new instance to keep this one
    //This makes sure that the object that I've named "Global Object" in my scene is carried out all the way to the end of the game
    //This way, I can store global variables in here if they need to be constant throughout the levels. eg. The settings

    public static GlobalControl Instance;
    void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

    private void Start()
    {
        Player.health = currentLoadHealth;
    }
}
