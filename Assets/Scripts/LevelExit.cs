using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelExit : MonoBehaviour
{
    
    
    //References
    [SerializeField] GameObject player;
    [SerializeField] Image blackImage;
    [SerializeField] GameObject popUp;
    CapsuleCollider2D playerCollider;
    CompositeCollider2D portalCollider;
    Animator imageAnimator;
    

    //Delay on exiting the level
    [SerializeField] float ExitDelaySeconds = 1f;

    //Checks if the player is colliding with the exit portal
    bool isColliding = false;

    // Start is called before the first frame update
    private void Start()
    {
        portalCollider = GetComponent<CompositeCollider2D>();
        playerCollider = player.GetComponent<CapsuleCollider2D>();
        imageAnimator = blackImage.GetComponent<Animator>();
        
    }

    // Update is called once per frame
    private void Update()
    {
        PopUp();
        PlayerCollision();        
    }

    private void PopUp()
    {
        //If there is already a popup that exists, and the player is still colliding with the door, then stop.
        if(GameObject.Find("UpArrow(Clone)") && isColliding) { return; }

        //If the player is colliding (and because of the previous line, there wont be a popup already existing)
        //Then make a popup - otherwise (meaning the player is no longer colliding with the door - then destroy the popup)
        if (isColliding)
        {           
            Instantiate(popUp);
        }
        else
        {
            Destroy(GameObject.Find("UpArrow(Clone)"));
        }
    }

    private void PlayerCollision()
    {
        //These links helped me figure out how to use the bounds of a trigger box as a collider, instead of only using the edges
        //https://docs.unity3d.com/ScriptReference/Bounds.Intersects.html
        //https://www.reddit.com/r/Unity3D/comments/80gp41/checking_if_a_collider_is_fully_inside_another_one/

        if (portalCollider.bounds.Intersects(playerCollider.bounds))
        {
            //Updates the variable to say "the player is colliding with the door!"
            isColliding = true; 
            //If the player presses W or Up Arrow while colliding with the door, then load the next level.
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) 
            {
                //This Player.locked is something I implemented in response to someone finding the "zombie bug" where the player would die
                //While the level is fading out to load the next level, and appear as a corpse in the next level, unable to move or do anything
                Player.locked = true;
                StartCoroutine(ExitDelay());
            }
        }
        else
        {
            //If the player ISN'T in the bounds of the door, then update the variable to say "the player isn't colliding with the door"
            isColliding = false;
        }
    }

    private IEnumerator ExitDelay()
    {
        //When the player enters the door, fade out and wait an amount of time  
        imageAnimator.SetBool("Fade",true);
        yield return new WaitForSecondsRealtime(ExitDelaySeconds);

        //Load next level
        
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        if (currentSceneIndex != 8)
        {
            GlobalControl.currentLoadHealth = Player.health;
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
        
        
    }

}
