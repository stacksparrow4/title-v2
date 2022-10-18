using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChest : MonoBehaviour
{ 

    //This is my old chest system that I rewrote in the NewChest script


    [SerializeField] GameObject[] ChestArray;
    [SerializeField] GameObject player;
    [SerializeField] Sprite openChestSprite;
    [SerializeField] GameObject popUp;
    CapsuleCollider2D playerBodyCollider;


    private bool chestIsOpen0 = false;
    private bool chestIsOpen1 = false;
    private bool chestIsOpen2 = false;

    private void Start()
    {
        playerBodyCollider = player.GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        CollisionDetection();
    }

    private void PopUp()
    {
        //If there is already a popup that exists, then stop.
        //Otherwise, make a popup appear
        if (GameObject.Find("E Popup(Clone)")) { return; }

        Instantiate(popUp);

    }

    //This system of detecting which chest has been opened is very inefficient and only allows for 3 chests per level.
    //If I need more than 3 chests, I will need to add a few lines of code.
    private void CollisionDetection()
    {
        //Here I chose to use layer masks - this would be more effective if I had a "Chest Controller" or something. But putting this script
        //on individual scripts made my life hell with so many references
        if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Chest")) || playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Chest1")) ||
            playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Chest2")))
        {
            PopUp();
        }
        else
        {
            Destroy(GameObject.Find("E Popup(Clone)"));
        }


        //This module is repeated for every individual layer, but basically if the player is touching the layer then make a popup appear
        //If they press E, the chest opens and gives health
       if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Chest")))
        {
            PopUp();
            if (Input.GetKeyDown(KeyCode.E) &&  !chestIsOpen0)
            {
                chestIsOpen0 = true;
                ChestArray[0].GetComponent<SpriteRenderer>().sprite = openChestSprite;
                
                if (Player.health == 2)
                {
                    Player.health = 3;
                }
                else if (Player.health == 1)
                {
                    Player.health = 2;
                }
            }
        }
        

        if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Chest1")))
        {
            PopUp();
            if (Input.GetKeyDown(KeyCode.E) && !chestIsOpen1)
            {
                chestIsOpen1 = true;
                ChestArray[1].GetComponent<SpriteRenderer>().sprite = openChestSprite;
                if (Player.health < 3)
                { Player.health += 1; }
            }            
        }
      

        if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Chest2")))
        {
            PopUp();
            if (Input.GetKeyDown(KeyCode.E) && !chestIsOpen2)
            {
                chestIsOpen2 = true;
                ChestArray[2].GetComponent<SpriteRenderer>().sprite = openChestSprite;
                if (Player.health < 3)
                { 
                    Player.health += 1; 
                }
                chestIsOpen2 = true;
            }
        }
       
    }
}
