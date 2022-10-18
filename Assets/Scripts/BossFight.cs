using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossFight : MonoBehaviour
{
    private bool Phase1;
    private bool Phase2;
    private bool Phase3;

    private bool bossDark = true;

    public static bool bossDead;
    public static bool bossRed;

    [SerializeField] GameObject Fireball;
    [SerializeField] GameObject spikeLayer1;
    [SerializeField] GameObject spikeLayer2;

    [SerializeField] GameObject FloorParticles;
    [SerializeField] GameObject FloorDetails;
    [SerializeField] GameObject PlatformParticles;

    [SerializeField] Slider healthSlider;
    [SerializeField] GameObject ExitPortal;

    [SerializeField] AudioSource bossIntro;

    [SerializeField] GameObject bigBossMan;
    [SerializeField] ParticleSystem defaultParticles;
    [SerializeField] ParticleSystem secondParticles;

    [SerializeField] GameObject orangeBack;
  

    private int bossHealth = 30;
    private float bossPosY;

    private void Start()
    {
        //Makes sure this variable is reset when the boss room is entered
        bossDead = false;
        //Sets the value of the visual health bar back to full
        healthSlider.value = bossHealth;
        //Makes sure the health bar isn't active when you first load in
        healthSlider.gameObject.SetActive(false);
        //Hide the boss under the map ready to be lifted up
        bigBossMan.transform.position = new Vector2(1299.19f, -28f);
        //Used to rise the boss up
        bossPosY = bigBossMan.transform.position.y;
    }

    private void Update()
    {
        //Make the boss sprite red if he's dead
        if (healthSlider.value <= 0)
        { bigBossMan.GetComponent<SpriteRenderer>().color = Color.red; }

        //Whenever the boss turns red, update the value of the health slider
        if (bossRed)
        { 
            bigBossMan.GetComponent<SpriteRenderer>().color = Color.red;
            healthSlider.value = bossHealth;
        }
        //If the boss isn't red, and it isn't in it's "dark" phase then reset it to white
        else if (!bossRed && !bossDark)
        { bigBossMan.GetComponent<SpriteRenderer>().color = Color.white; }

        //Developer cheat to kill the boss faster. When T is pressed, get rid of 1 health
        if(Input.GetKeyDown(KeyCode.T))
        {
            bossHealth -= 1;
            healthSlider.value = bossHealth;
        }

        //If the door is closed and the boss' position isn't in the centre of the screen, move the boss up incrementally until he is at the right position
        if(DoorClose.bossStarted && bigBossMan.transform.position.y < -15.84f)
        {
            bigBossMan.transform.position = new Vector2(1299.19f, bossPosY + 0.03f);
            bossPosY = bigBossMan.transform.position.y;
        }

        
        //Similarly if the boss is dead, slowly make him go back down and turn off the particles, the music, and the spikes
        if (bossDead && bigBossMan.transform.position.y > -35f)
        {
            bigBossMan.transform.position = new Vector2(1299.19f, bossPosY - 0.1f);
            bossPosY = bigBossMan.transform.position.y;

            defaultParticles.Stop();
            secondParticles.Stop();
            bossIntro.Stop();
            spikeLayer1.SetActive(false);
            spikeLayer2.SetActive(false);
            ExitPortal.SetActive(true);
            return; 
        }
        //If the boss is dead, and it's off screen, destroy the boss object, then destroy the boss controller
        if (bigBossMan.transform.position.y < -30 && bossDead)
        {
            PlayerPrefs.SetInt("bossClear",1);
            Destroy(bigBossMan);
            Destroy(gameObject);
        }
        //If the door is closed, and its not phase 1, not phase 2, and not phase 3, then it must be the intro sequence
        //Activate the health bar and begin PhaseOne
        if(DoorClose.bossStarted && !Phase1 && !Phase2 && !Phase3)
        {
            healthSlider.gameObject.SetActive(true);
            Phase1 = true;
            PhaseOne();            
        }
        //If phase one is over, then start phase 2 by activating the floor particles and beginning PhaseTwo
        else if(Phase1 && Phase2 && !Phase3)
        {
            Phase1 = false;
            FloorParticles.SetActive(true);
            FloorDetails.SetActive(true);
            PhaseTwo();
        //Once phase 2 is over, start the platform particles and begin PhaseThree
        }
        else if(!Phase1 && Phase2 && Phase3)
        {
            Phase2 = false;
            PlatformParticles.SetActive(true);
            PhaseThree();
        }
    }

    //Delay the start of the fight by 8.8 seconds to sync with the music, then start the music and lock the player's movements
    private void PhaseOne()
    {
        //First parameter is the name of the function being called, the second parameter is how long to delay it by
        Invoke("PhaseOne2", 8.8f);
        bossIntro.Play();
        Player.locked = true;
    }
    //After the 8.8 seconds, the player is no longer locked, and then after 5 seconds RedEyes is called and Fireballs begin shooting every 3 seconds
    private void PhaseOne2()
    {
        Player.locked = false;
        Invoke("RedEyes",5f);
        //First parameter is the name of the function being called, second parameter is how long to delay it by, the third parameter is how often this is repeated
        InvokeRepeating("FireBalls", 5f, 3f);
    }

    //The boss is no longer darkened and the animation starts, the background particles are also activated
    private void RedEyes()
    {
        bossDark = false;
        bigBossMan.GetComponent<Animator>().SetTrigger("Phase1");
        defaultParticles.gameObject.SetActive(true);
    }
    //Delay the start of phase 2 to let the particles sit there for 5 seconds
    private void PhaseTwo()
    { Invoke("PhaseTwo2",5f); }

    private void PhaseTwo2()
    {
        //Activate the floor spikes
        spikeLayer1.SetActive(true);
        //Turn off the particle effect
        FloorParticles.SetActive(false);
        //Set the animation to the Phase 2 one
        bigBossMan.GetComponent<Animator>().SetTrigger("Phase2");
        //Activate more background particles
        secondParticles.gameObject.SetActive(true);
        //Start shooting fireballs again
        InvokeRepeating("FireBalls", 2f, 3f);
    }

    //Delay the start of phase 2 to let the particles sit there for 5 seconds
    private void PhaseThree()
    { Invoke("PhaseThree2", 5f); }

    private void PhaseThree2()
    {
        //Activate the platform spikes
        spikeLayer2.SetActive(true);
        //Turn off the particle effect
        PlatformParticles.SetActive(false);
        //Set the animation to the Phase 3 one
        bigBossMan.GetComponent<Animator>().SetTrigger("Phase3");
        //Change the colour of both layers of particles
        defaultParticles.startColor = Color.red;
        secondParticles.startColor = Color.yellow;
        //Set the background to an orange colour
        orangeBack.gameObject.SetActive(true);
        //Start shooting fireballs again
        InvokeRepeating("FireBalls", 2f, 3f);
    }

    
    private void FireBalls()
    {
        //If the boss health reaches the threshhold of Phase 1, move onto Phase 2 and stop the fireballs
        if(bossHealth == 20 && !Phase2) 
        {
            Phase2 = true;
            CancelInvoke("FireBalls");
            return; 
        }
        //If the boss health reaches the threshhold of Phase 2, move onto Phase 3 and stop the fireballs
        else if (bossHealth == 10 && Phase2 && !Phase3)
        {
            Phase3 = true;
            CancelInvoke("FireBalls");
            return;
        }
        //If the boss health reaches the threshhold of Phase 3, set the BossDead variable to true and stop the fireballs
        else if (bossHealth == 0 && Phase3)
        {
            bossDead = true;
            CancelInvoke("FireBalls");
            return;
        }
        //Launches 10 fireballs on every iteration
        int index = 0;
        while (index < 10)
        {
            //Spawns the fireball between 2 random x values
            float spawnPointX = Random.Range(1284.47f, 1312.7f);
            //Spawns the fireball between 2 random y values
            float spawnPointY = Random.Range(-5, 20);
            //Spawns a fireball at the random position calculated
            Instantiate(Fireball, new Vector3(spawnPointX, spawnPointY, 1f), Quaternion.identity);
            index += 1;

            //If the spawnpoint is within these x values, cause the boss to take damage - only if it's above the threshhold of the particular phase
            if (spawnPointX > 1297 && spawnPointX < 1301.5 && bossHealth > 20 && !Phase2)
            { bossHealth -= 1; }

            else if (spawnPointX > 1297 && spawnPointX < 1301.5 && bossHealth > 10 && Phase2)
            { bossHealth -= 1; }

            else if (spawnPointX > 1297 && spawnPointX < 1301.5 && bossHealth > 0 && Phase3)
            { bossHealth -= 1; }
        }
        
    }
}
