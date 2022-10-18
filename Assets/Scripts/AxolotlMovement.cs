using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxolotlMovement : MonoBehaviour
{
    //The speed the axolotl moves
    [SerializeField] float walkSpeed = 2f;

    //Rigidbody of the enemy
    Rigidbody2D myRigidBody;

    //Front trigger detecting the wall
    BoxCollider2D triggerCollider;

    //Full enemy hitbox
    CapsuleCollider2D mainBody;

    //Detects if the front trigger has touched a wall
    bool touchingtheFloor = false;

    // Start is called before the first frame update
    void Start()
    {
        //Retrieves references from the gameobject and puts them into a variable
        myRigidBody = GetComponent<Rigidbody2D>();
        triggerCollider = GetComponent<BoxCollider2D>();
        mainBody = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Updates the variable of touchingTheWall to check whether or not the enemy needs to turn around
        TouchingWall();
        //Controls enemy movement and bouncing off the wall
        Walk();
        //Disables the enemys' hitboxes when the player is invincible
        DisablePlayerCollision();
        
    }

    private void TouchingWall()
    {   
        //If the front collider is touching the wall, set a boolean to true - if not, set it to false
        if (triggerCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        { touchingtheFloor = true;  }
        else
        { touchingtheFloor = false;  }

        //If the axolotl's trigger box leaves the floor, then flip the sprite
        if (!touchingtheFloor)
        { transform.localScale = new Vector2(-(Mathf.Sign(myRigidBody.velocity.x)), 1f); }
    }

    private void DisablePlayerCollision()
    {
        //Calls a global variable to determine if the player is invincible and if the player is alive
        //If the player is invincible, or if the player is dead, disable the collider which disables collision between enemies and the player
        if (Player.isInvincible || !Player.isAlive)
        { mainBody.enabled = false; }
        else
        { mainBody.enabled = true;  }
    }

    private void Walk()
    {
        //Checks if enemy is facing right, if they are - move right, if they aren't, move "negative right" (left)
        if (isFacingRight())
        { myRigidBody.velocity = new Vector2(walkSpeed, 0f); }
        else
        { myRigidBody.velocity = new Vector2(-walkSpeed, 0f); }

    }

    bool isFacingRight()
    {
        //Checks if the scale on the object is positive (meaning its facing right)
        return transform.localScale.x > 0;
    }    
}
