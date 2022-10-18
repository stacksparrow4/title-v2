using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameSession : MonoBehaviour
{
    //References
    [SerializeField] Image blackImage;
    [SerializeField] Sprite FullHeart;
    [SerializeField] Sprite EmptyHeart;

    [SerializeField] GameObject heart1;
    [SerializeField] GameObject heart2;
    [SerializeField] GameObject heart3;

    private void Update()
    {
        //This is to control that the player always has the right amount of health showing.
        //Swaps between FullHeart sprite to EmptyHeart sprite.
        if (Player.health ==3)
        {
            heart1.GetComponent<Image>().sprite = FullHeart;
            heart2.GetComponent<Image>().sprite = FullHeart;
            heart3.GetComponent<Image>().sprite = FullHeart;
        }
        if (Player.health == 2)
        {
            heart1.GetComponent<Image>().sprite = FullHeart;
            heart2.GetComponent<Image>().sprite = FullHeart;
            heart3.GetComponent<Image>().sprite = EmptyHeart;
        }
        if (Player.health == 1)
        {
            heart1.GetComponent<Image>().sprite = FullHeart;
            heart2.GetComponent<Image>().sprite = EmptyHeart;
            heart3.GetComponent<Image>().sprite = EmptyHeart;
        }
        if (Player.health <= 0)
        {
            heart1.GetComponent<Image>().sprite = EmptyHeart;
            heart2.GetComponent<Image>().sprite = EmptyHeart;
            heart3.GetComponent<Image>().sprite = EmptyHeart;
        }

       
    }

    //Called in the player script when the player dies - basic respawn mechanic
    public void ResetGameSession()
    {
        StartCoroutine(DeathDelay());       
    }

    public IEnumerator DeathDelay()
    {
        //Delays death for a bit
        yield return new WaitForSecondsRealtime(1f);
        //Fades to black and fades the health text to black
        blackImage.GetComponent<Animator>().SetBool("Fade", true);
        yield return new WaitForSecondsRealtime(.5f);
        Player.health = 3;
        //If the player has selected hardcore mode, then send them back to the main menu. If not, then load the start of the level
        if (GlobalControl.hardCoreMode == true)
        {
            //Loads the main menu
            SceneManager.LoadScene(0);
        }
        else
        {
            //Load same level
            Player.health = GlobalControl.currentLoadHealth;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
           
        }
        
        
        //Destroys the gamesession - ready for a new one
        Destroy(gameObject);
        //Resets the player to being alive again
        Player.isAlive = true;
    }
}
