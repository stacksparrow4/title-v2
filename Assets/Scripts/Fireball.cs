using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private float currentX;
    private float currentY;
    private float xMovement;
    private void Start()
    {
        //Wherever the fireball is spawned, is then cached to be used later
        currentX = transform.position.x;
        currentY = transform.position.y;
        xMovement = Random.Range(-2f,2f);
    }
    private void Update()
    {
        BossRed();
        MoveDown();
    }

    //If the fireball is inbetween 2 y values, then turn the boss sprite red to signify a hit
    private void BossRed()
    {
        if (currentY < -10 && currentY > -12)
        { BossFight.bossRed = true; }
        else
        { BossFight.bossRed = false; }
    }

    //Every frame, until the y value is <= -25.1 (which is near the bottom of the screen)
    //Move down by a set amount
    //The else if bascially says to despawn any remaining fireballs once the boss has died, or if they go off screen - destroy them as well
    private void MoveDown()
    {
        if (transform.position.y > -25.1)
        {
            transform.position = new Vector2(currentX + xMovement, currentY - 0.2f);
            currentY = transform.position.y;
        }
        else if(BossFight.bossDead || transform.position.y < -24)
        {
            Destroy(gameObject);
        }
    }

   
}
