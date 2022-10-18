using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManagement : MonoBehaviour
{
    //References
    public GameObject myMainCamera;

    //When the player enters a room, set that camera as the active camera
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            myMainCamera.SetActive(true);
        }
    }
    //When the player exits a room, disable that camera (hopefully after exiting, they immediately enter a new room for a new camera to take over)
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            myMainCamera.SetActive(false);
        }
    }
}
