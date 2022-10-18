using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DoorClose : MonoBehaviour
{
    private Vector2 startingPosition;
    [SerializeField] GameObject Door;
    private bool closeDoor;
    public static bool bossStarted;
   

    // Start is called before the first frame update
    void Start()
    {
        // Makes sure that when the boss scene is loaded, that this bossStarted variable is set to false
        bossStarted = false;
        startingPosition = Door.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //If the door has to be closed, then move the door to the closed position, then set the variable to false so it only does this method once
        //Then signify that the boss has started, then destroy the trigger that the script is linked to - to delete the script
        if(closeDoor)
        {
            Door.transform.position = new Vector2(startingPosition.x, -21);
            closeDoor = false;
            bossStarted = true;
            Destroy(gameObject);
        }
    }

    
    //If the player enters the trigger bounds of the boss starting trigger
    //(Basically if they enter the boss room) - then close the door
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            closeDoor = true;
        }
    }
}
