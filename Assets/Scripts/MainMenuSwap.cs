using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSwap : MonoBehaviour
{
    private bool topRoom = false;
    [SerializeField] GameObject UIElements;

    //This script is used in the main menu to transition between the menu working, and the menu being hidden for the top room
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the player has entered the top room - then set the variable to true
        if(collision.CompareTag("Player"))
        {
            topRoom = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //If they exit the top room - set the variable to false
        if (collision.CompareTag("Player"))
        {
            topRoom = false;
        }
    }

    private void Update()
    {
        //If the player is in the top room, turn off the UI, else - turn it back on
        if (topRoom)
        {
            UIElements.SetActive(false);
        }
        else
        {
            UIElements.SetActive(true);
        }
    }



}
