using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    [SerializeField] float platformSpeed;
    bool isFacingRight = true;
    bool isFacingUp = true;


    [SerializeField] GameObject LeftEdge;
    [SerializeField] GameObject RightEdge;

    [SerializeField] bool horizontal;
   

    void Update()
    {
        Move();
        EdgeBounce();
    }

    private void EdgeBounce()
    {
        //This horizontal variable determines if the platform is an up and down, or a left to right platform
        if(horizontal)
        {
            // Move to the left or right - then IF the x position ever reaches a certain amount, switch which way you're going.
            // Same applies to vertical platforms
            if (transform.position.x >= RightEdge.transform.position.x)
            { isFacingRight = false; }

            if(transform.position.x <= LeftEdge.transform.position.x)
            { isFacingRight = true; }
        }
        else
        {
            if (transform.position.y >= RightEdge.transform.position.y)
            { isFacingUp = false; }

            if (transform.position.y <= LeftEdge.transform.position.y)
            { isFacingUp = true; }
        }
    }

    private void Move()
    {
        if (horizontal)
        {
            //Checks if platform is facing right, if they are - move right, if they aren't, move "negative right" (left)
            //Same with vertical, up and "negative up" (down)
            if (isFacingRight)
            { GetComponent<Rigidbody2D>().velocity = new Vector2(platformSpeed, 0f); }
            else
            { GetComponent<Rigidbody2D>().velocity = new Vector2(-platformSpeed, 0f); }
        }
        else
        {
            if (isFacingUp)
            { GetComponent<Rigidbody2D>().velocity = new Vector2(0f, platformSpeed); }
            else
            { GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -platformSpeed); }
        }
    }


    

}
