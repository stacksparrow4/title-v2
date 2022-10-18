using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadSave : MonoBehaviour
{
    //https://www.red-gate.com/simple-talk/dotnet/c-programming/saving-game-data-with-unity/#:~:text=Unity%20provides%20two%20ways%20to,%2C%20and%20you're%20done.
    //https://www.youtube.com/watch?v=donIirlj074
    //https://answers.unity.com/questions/1576989/playerprefs-multiple-save-slots.html

    public static int slot;
    public static int deleteSlot;
    [SerializeField] GameObject player;

    [SerializeField] Text slotText1;
    [SerializeField] Text slotText2;
    [SerializeField] Text slotText3;

    [SerializeField] GameObject[] chestArr;
    private int[] chestStatesArr;

    public static float playerPosX;
    public static float playerPosY;
    public static bool loaded = false;


    private void Start()
    {
        //If there's any chests in the level, then set the states array to the same length as the chest array length
        if (chestArr.Length != 0)
        { chestStatesArr = new int[chestArr.Length]; }

        //While loop goes through and checks whether or not a chest should be open or closed
        //Checks the playerprefs value for the chest state. If the chest state = 1 it means the chest is open, else its closed
        int index = 0;
        while (index < chestArr.Length)
        {
            if (PlayerPrefs.GetInt("chest" + index + "State_" + slot, chestStatesArr[index]) == 1)
            {
                //Sets the gameobject tag to chestopen, which changes the state of the chest within the chest script
                chestArr[index].tag = "ChestOpen";
            }
            else
            { chestArr[index].tag = "ChestClosed"; }
            index += 1;
        }

    }
    //These are used to identify which "trash can icon" the player clicked, and which corresponding save slot will be deleted.
    public void DeleteSlot1()
    {
        deleteSlot = 1;
        Delete();
    }

    public void DeleteSlot2()
    {
        deleteSlot = 2;
        Delete();
    }

    public void DeleteSlot3()
    {
        deleteSlot = 3;
        Delete();
    }

    //Deletes the PlayerPrefs data related to whichever deleteSlot was assigned
    private void Delete()
    {
        PlayerPrefs.DeleteKey("health_" + deleteSlot);
        PlayerPrefs.DeleteKey("currentlevel_" + deleteSlot);
        PlayerPrefs.DeleteKey("playerPositionX_" + deleteSlot);
        PlayerPrefs.DeleteKey("playerPositionY_" + deleteSlot);



        //This while loop is actually a new addition since I built the final version that I just noticed while writing these comments
        // You'll notice if you play the game from the exported version, if you open a chest, then save, then exit and delete the file.
        // The chest you opened will still be open... This issue is fixed in the developer version but I didn't want to re-export it for all
        // the different platforms and rename it to V2.1 because that wasn't as catchy! >:D
        int index = 0;
        while (index < 20)
        {
            if (PlayerPrefs.HasKey("chest" + index + "State_" + deleteSlot))
            { PlayerPrefs.DeleteKey("chest" + index + "State_" + deleteSlot); }
            index += 1;
        }
    }
    //If there is a value saved in the PlayerPrefs that has the key _1, _2, or _3 at the end, it means that the saveslot has data in it
    //I just use the health key as an example, but I could've used any value that has been saved
    //This basically just says. If a save slot exists, make the label say "CONTINUE" if it doesn't have any save data on it, make it say
    //"NEW GAME"
    private void Update()
    {
        if (PlayerPrefs.HasKey("health_1"))
        {
            slotText1.text = "CONTINUE";
        }
        else
        {
            slotText1.text = "NEW GAME";
        }

        if (PlayerPrefs.HasKey("health_2"))
        {
            slotText2.text = "CONTINUE";
        }
        else
        {
            slotText2.text = "NEW GAME";
        }

        if (PlayerPrefs.HasKey("health_3"))
        {
            slotText3.text = "CONTINUE";
        }
        else
        {
            slotText3.text = "NEW GAME";
        }
    }

    //Used for the "Delete all Save Data" button, just gets rid of everything stored in PlayerPrefs
    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    //Assigned to each button to determine which save slot we're trying to access
    public void LoadState1()
    {
        slot = 1;
        Load();
    }
    public void LoadState2()
    {
        slot = 2;
        Load();
    }
    public void LoadState3()
    {
        slot = 3;
        Load();
    }

    private void Load()
    {
        //Basically if the key exists, meaning we're trying to load, then do stuff
        if (PlayerPrefs.HasKey("health_" + slot))
        {
            //Takes the values for health and current level and puts them in temporary variables to be loaded
            int health = PlayerPrefs.GetInt("health_" + slot);
            int current_level = PlayerPrefs.GetInt("currentlevel_" + slot);
            //These are loaded when the player loads into the scene
            playerPosX = PlayerPrefs.GetFloat("playerPositionX_" + slot);
            playerPosY = PlayerPrefs.GetFloat("playerPositionY_" + slot);
            //Makes sure the values are only loaded once
            loaded = true;
            //Assigns the variables to the values of the temporary ones just loaded
            Player.health = health;
            SceneManager.LoadScene(current_level);
        }
        else
        {
            //Else just load the first level, making sure the player health is 3
            Player.health = 3;
            SceneManager.LoadScene(1);
        }
    }

    public void Save()
    {
        //Saves all the relevant values in PlayerPrefs

        PlayerPrefs.SetInt("health_" + slot, Player.health);
        PlayerPrefs.SetInt("currentlevel_" + slot, SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.SetFloat("playerPositionX_" + slot, player.transform.position.x);
        PlayerPrefs.SetFloat("playerPositionY_" + slot, player.transform.position.y);

        //If no chests exist, then you don't need to save anything. The rest of the parameters will always exist
        if (chestArr.Length != 0)
        {
            int index = 0;
            while (index < chestArr.Length)
            {
                //With player prefs I can only store values as ints or floats. And I needed a boolean value to check if the
                //chest was open or closed. So based on if the chest is open, I set the int as a 1, and if it was closed I made it a 0
                //Basically, if the tag on the GameObject is "ChestOpen" then set the array value to a 1 or a 0
                if (chestArr[index].tag == "ChestOpen")
                {
                    chestStatesArr[index] = 1;
                    index += 1;
                }
                else
                {
                    chestStatesArr[index] = 0;
                    index += 1;
                }

                int index1 = 0;
                while (index1 < chestArr.Length)
                {
                    //Once the values are stored in the array, it searches through that array and assigns the values 
                    //to the PlayerPrefs to be saved
                    PlayerPrefs.SetInt("chest" + index1 + "State_" + slot, chestStatesArr[index1]);
                    index1 += 1;
                }

            }





        }


    }
}
