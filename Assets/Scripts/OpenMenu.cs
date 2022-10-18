using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OpenMenu : MonoBehaviour
{

    //All these references were going to be used to control all the aspects of the menu system but didnt end up getting around to it

    //References
    [SerializeField] GameObject PanelContainer;
    [SerializeField] GameObject ButtonContainer;

    [SerializeField] GameObject VideoPanel;
    [SerializeField] GameObject MidWayPanel;
    [SerializeField] GameObject PlayPanel;

    [SerializeField] GameObject HeartsContainer;
    //
    //
    //
    [SerializeField] GameObject AudioPanel;
        [SerializeField] Slider masterVolumeSlider;
        [SerializeField] Slider musicVolumeSlider;
        [SerializeField] Slider interfaceVolumeSlider;
        [SerializeField] Slider ambienceVolumeSlider;
    [SerializeField] GameObject ControlsPanel;
        //
        //
        //
    [SerializeField] GameObject AccessibilityPanel;
        [SerializeField] Toggle hardcoreToggle;
    //
    //  

    [SerializeField] GameObject[] CreditsPanels;

    //Temp Values - used to hold the menu values temporarily before being saved when the menu closes.
    private static float tempMasterValue;
    private static float tempMusicValue;
    private static float tempInterfaceValue;
    private static float tempAmbienceValue;
    private static bool TempHardcoreToggle;

    //Tracks the state of the menu at any given time - open or closed
    public static bool menuIsOpen = false;
    public static bool playMenuIsOpen = false;

    //Makes sure the video panel will only be loaded once on first open
    private bool menuHasBeenLoaded = false;

    //Called when the back to menu button is pressed - sends back to menu and toggles the menu off
    public void BackToMain()
    {
        //This is where I'd add saving

        SceneManager.LoadScene(0);
        menuIsOpen = false;
    }

    //Called when the exit button is pressed - closes the game
    public void ExitGame()
    {
        Application.Quit();
    }

   

    // Update is called once per frame
    private void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex != 8)
        { DoorClose.bossStarted = false; }

        if(BossFight.bossDead && CreditsPanels.Length > 0)
        { 
            CreditsPanels[0].SetActive(true);
            BossFight.bossDead = false;
        }

        DetectEscape();
        if (!playMenuIsOpen)
        {
            PlayPanel.SetActive(false);
        }
        //When the menu is open, turn on the object containing the menus, turn off the buttons (this is only applicable on the main menu screen)
        if (menuIsOpen)
        {
            
            //Only does this once, as soon as the menu is loaded. This was created to fix an issue where the videopanel would always be active behind other panels
            //Because it is being setactive every frame
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                PanelContainer.SetActive(true);
                ButtonContainer.SetActive(false);
                if (!menuHasBeenLoaded)
                {
                    VideoPanel.SetActive(true);
                    LoadPlayerPrefs();
                    menuHasBeenLoaded = true;
                }
            }
            else
            {
                if (!menuHasBeenLoaded)
                {
                    MidWayPanel.SetActive(true);
                    HeartsContainer.SetActive(false);
                    LoadPlayerPrefs();
                    menuHasBeenLoaded = true;
                }
            }
            
            
        //Stops time while the menu is open
            Time.timeScale = 0f;
        }
        else
        {
        //Turns off all panels and makes the start buttons visible again
            PanelContainer.SetActive(false);
            AudioPanel.SetActive(false);
            ControlsPanel.SetActive(false);
            AccessibilityPanel.SetActive(false);
            MidWayPanel.SetActive(false);
            ButtonContainer.SetActive(true);
            HeartsContainer.SetActive(true);
        //Resets this variable to be used next time the menu is opened
            menuHasBeenLoaded = false;
        //Sets time back to normal
            Time.timeScale = 1f;           
        }
    }

    private void DetectEscape()
    {
        //When the player presses escape while they are alive - the boolean is updated
        if (Input.GetKeyDown(KeyCode.Escape) && Player.isAlive && !Player.isInvincible && !DoorClose.bossStarted)
        {
        //If escape is pressed when the menu is open, close it.
        //If escape is pressed when the menu is closed, open it.
            if (menuIsOpen || playMenuIsOpen)
            {               
                menuIsOpen = false;
                playMenuIsOpen = false;
            }
            else
            {               
                menuIsOpen = true;  
            }
        }
    }

        //Saves temp values as the value of each object in the menu
        //Once temp values are saved, they are transferred to another script - values are stored there to be used over multiple scenes.
        //Talked more about this in GlobalControl.cs 
        //This method is called every time something changes in the menu - must be public to be accessed in the inspector
    public void SavePlayerPrefs()
    {
        tempAmbienceValue = ambienceVolumeSlider.value;
        GlobalControl.savedAmbienceValue = tempAmbienceValue;

        tempMusicValue = musicVolumeSlider.value;
        GlobalControl.savedMusicValue = tempMusicValue;

        tempMasterValue = masterVolumeSlider.value;
        GlobalControl.savedMasterValue = tempMasterValue;

       
        tempInterfaceValue = interfaceVolumeSlider.value;
        GlobalControl.savedInterfaceValue = tempInterfaceValue;

        TempHardcoreToggle = hardcoreToggle.isOn;
        GlobalControl.hardCoreMode = TempHardcoreToggle;
        
    }

        //Whenever the menu is loaded, this will take the values from the Global Object and put them in the menu
    private void LoadPlayerPrefs()
    {
        //If this is the first time the player has loaded the menu, show them the default values of the slider
        //If it isn't the first time they have opened the menu, set it to the saved values
        if (!GlobalControl.menuDefaultsChanged)
        {
            masterVolumeSlider.value = 1f;
            musicVolumeSlider.value = 1f;
            interfaceVolumeSlider.value = 1f;
            ambienceVolumeSlider.value = 1f;
            hardcoreToggle.isOn = false;

           // Debug.Log("Global Defaults have been set");
            GlobalControl.menuDefaultsChanged = true;
        }
        else
        {
           // Debug.Log("The value on the slider is what is saved in the global variable");

            musicVolumeSlider.value = GlobalControl.savedMusicValue;
            //Debug.Log("Saved music value: " + GlobalControl.savedMusicValue);

            masterVolumeSlider.value = GlobalControl.savedMasterValue;
           // Debug.Log("Saved master value: " + GlobalControl.savedMasterValue);

            interfaceVolumeSlider.value = GlobalControl.savedInterfaceValue;
          //  Debug.Log("Saved interface value: " + GlobalControl.savedInterfaceValue);

            ambienceVolumeSlider.value = GlobalControl.savedAmbienceValue;
           // Debug.Log("Saved ambience value: " + GlobalControl.savedAmbienceValue);

            hardcoreToggle.isOn = GlobalControl.hardCoreMode;

        }
    }



}
