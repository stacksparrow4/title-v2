using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewChests : MonoBehaviour
{
    private bool colliding = false;
    private bool opened = false;

    [SerializeField] Sprite closedSprite;
    [SerializeField] Sprite openSprite;
    [SerializeField] GameObject popUp;

    //I realised that the chests don't actually need 2 colliders, I can just use one trigger in the general area
    //Using OnTriggerEnter and OnTriggerExit I set a trigger for the player to walk into around the chest
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            colliding = true;
            //Makes an instance of the E Popup in the position just above the chest using the same rotation as the chest.
            if (GameObject.Find("E Popup(Clone)") || opened) { return; }
            Instantiate(popUp, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.identity);
        }
    }
    //Destroys the popup if the player leaves the area of the chest
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            colliding = false;
            Destroy(GameObject.Find("E Popup(Clone)"));
        }
    }
    
    private void Update()
    {
        //If the player is in the area of the chest and presses E, then the chest will open and give the player one health
        if (colliding == true)
        {
            if (Input.GetKeyDown(KeyCode.E) && opened == false)
            {
                opened = true;
                //Destroys the E popup when the chest is opened
                Destroy(GameObject.Find("E Popup(Clone)"));
                if (Player.health == 3)
                {
                    return;
                }
                else if (Player.health == 2)
                {
                    Player.health = 3;
                }
                else if (Player.health == 1)
                {
                    Player.health = 2;
                }
            }
        }
        //If the variable "opened" is true, or the tag of the object is ChestOpen, then set the sprite to the openchest sprite
        //This tag system helps me to load save states of chests
            if (opened == true || gameObject.tag == "ChestOpen")
            {
                GetComponent<SpriteRenderer>().sprite = openSprite;
                gameObject.tag = "ChestOpen";
                opened = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = closedSprite;
                gameObject.tag = "ChestClosed";
                opened = false;
            }
        
    }
}
