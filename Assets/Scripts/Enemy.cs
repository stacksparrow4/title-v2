using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

public class Enemy : MonoBehaviour
{
    //Collider the player is able to hit to jump on the enemy
    BoxCollider2D bouncePad;

    // Start is called before the first frame update
    private void Start()
    {
        //Retrieves references from the gameobject and puts them into a variable
        bouncePad = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        StopPlayerCollision();
    }

    private void StopPlayerCollision()
    {
        //If the player is able to bounce, make the bouncepad collideable (is that a word? - able to collide with)
        if(Player.ableToBounceOnEnemies && Player.isAlive)
        {           
            bouncePad.enabled = true;
        }
        else
        {          
            bouncePad.enabled = false;
        }        
    }

    //When something enters the bouncepad trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the collision was the player &the player is "Able to bounce" then make the player kill the enemy and destroy the enemy
        if (collision.CompareTag("Player") && Player.ableToBounceOnEnemies)
        {
            //Global Variable https://answers.unity.com/questions/699565/how-to-get-a-variable-value-from-another-scriptc.html
            Player.killedAnEnemy = true;
            //Destroying a parent object https://answers.unity.com/questions/275343/destroy-parent-of-child-gameobject.html
            Destroy(transform.parent.gameObject);            
        }   
    }   

}
