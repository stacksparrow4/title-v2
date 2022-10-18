using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class Player : MonoBehaviour
{ 
    //Global variables
    public static bool killedAnEnemy;
    public static bool ableToBounceOnEnemies = true;
    public static bool isInvincible = false;
    public static bool isAlive = true;
    public static int health = 3;
    public static bool locked;

    //Parameters
    float startRunSpeed;
    float endRunSpeed = 8;
    float runSpeed;
    [SerializeField] float jumpSpeed = 50f;
    [SerializeField] float climbSpeed = 50f;
    [SerializeField] float wallSlidingSpeed = 10f;
    [SerializeField] float wallJumpSpeed = 50f;
    [SerializeField] float enemyBounceHeight = 50f;
    [SerializeField] float invincibilityDurationInSeconds = 2f;
    [SerializeField] float startDashTimer;
    [SerializeField] float dashSpeed;

    float dashDirection;
    float currentDashTimer;
    float numberOfWallJumps = 0f;
    float gravityScaleAtStart;
    

    // States
    bool wallSliding = false;
    bool dashing = false;
    bool ableToDash = true;


    //References / Cached Objects
    Rigidbody2D playerRigidbody;
    Animator playerAnimator;
    CapsuleCollider2D playerBodyCollider;
    BoxCollider2D myFeetCollider;



    // Start is called before the first frame update
    void Start()
    {
        locked = false;
        isInvincible = false;
        //Get references from gameobjects and store them in variables
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();

        //Gets the gravity scale before climbing / getting in water so it can be set to default
        gravityScaleAtStart = playerRigidbody.gravityScale;

        //Sets the starting runspeed
        startRunSpeed = 3f;
        //If this player is being loaded from a save, set their location to the one being passed in from the LoadSave script
        if(LoadSave.loaded == true)
        {
            transform.position = new Vector3(LoadSave.playerPosX, LoadSave.playerPosY);
            LoadSave.loaded = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //If the player isn't alive, make the sprite red and dont let any other methods run
        if (!isAlive)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            return;
        }
        //If the player isn't invincible, let them take damage
        if (!isInvincible)
        { Damage(); }
        Bounceable();
        EnemyKilled();
        ClimbLadder();
        WallSlide();
        Fall();
        Accelerate();
        Run();
        Jump();
        WallJump();
        FlipSprite();
        Dash();
        if (locked)
        { Locked(); }

    }
        
    //If the player has been locked, then turn off all possible animations, set the velocity to 0
    //and make the player face to the right
    //This is used in two places, the first is when exiting a level, so the zombie bug can't happen (this is explained in the LevelExit script)
    //And the second place the player is locked, is in the intro sequence for the boss fight
    private void Locked()
    {       
            playerAnimator.SetBool("Jumping", false);
            playerAnimator.SetBool("Running", false);
            playerAnimator.SetBool("Climbing", false);
            playerAnimator.SetBool("Dashing", false);
            playerAnimator.SetBool("Wall Slide", false);
            playerRigidbody.velocity = new Vector2(0,0);
            transform.localScale = new Vector2(1,1);
    }

    private void Damage()
    {
        // Acts as a second catch to make sure players cannot take damage when they are dead.
        if (!isAlive) { return; }

        //If the collider on my feet, or on my body is touching an object with the layer "Enemy" or "Hazard" or "Deadly" then the player should take damage
        if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")) || myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")) ||
            playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Deadly")) || myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Deadly")) ||
           playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Hazard")) || myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Hazard")))
        {
            // If it's an enemy or a hazard that you've touched - minus 1 health
            if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")) || myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")) 
                || playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Hazard")) || myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Hazard")))
            { health -= 1; }
            // If it's "deadly", it's an instant kill. Going out of bounds will also count as a hazard.
            if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Deadly")) || myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Deadly")))
            { health -= 3; }

            //Make the sprite red when the player takes damage
            GetComponent<SpriteRenderer>().color = Color.red;
            //Bounces player up a bit when they take damage
            GetComponent<Rigidbody2D>().velocity = new Vector2(1f * Mathf.Sign(playerRigidbody.velocity.x), 10f);
            //If the player's health drops to less than 0 or 0, then set the bool of "is alive" to false and set the animation to being dead
            if (health <= 0)
            {
                isAlive = false;
                playerAnimator.SetTrigger("Dead");
                //Calls the ResetGameSession function from the GameSession object
                FindObjectOfType<GameSession>().ResetGameSession();
                return;
            }
            //If the player is still alive after taking damage, start the invincibility frames
            if (isAlive)
            { StartCoroutine(Invincibility()); }

        }
    }

    /* Setting invincibility frames - referenced from https://www.aleksandrhovhannisyan.com/blog/invulnerability-frames-in-unity/
    Some code from the Invincibility() function and also some calling of coroutine in the Damage() function.*/
    private IEnumerator Invincibility()
    {
        //If the player is alive, set the player to invincible and wait a few seconds, then set it back to false
        if (isAlive)
        {
            isInvincible = true;
            StartCoroutine(flashDelay());

            yield return new WaitForSeconds(invincibilityDurationInSeconds);

            isInvincible = false;
        }
    }

    private IEnumerator flashDelay()
    {
        StartCoroutine(whitecolor());
        yield return new WaitForSeconds(.5f);
        StartCoroutine(spriteFlash());
    }

    //Waits for a second before turning the sprite back to the default colour
    private IEnumerator whitecolor()
    {
        yield return new WaitForSeconds(1f);
        if (isAlive) { GetComponent<SpriteRenderer>().color = Color.white; }
    }

    //Makes the sprite flash transparent then back to normal, 3 times
    //I can use a loop for this - come back to it
    private IEnumerator spriteFlash()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(0.3f);
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1);
        yield return new WaitForSeconds(0.3f);
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(0.3f);
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1);
        yield return new WaitForSeconds(0.3f);
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(0.3f);
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1);
    }

    private void Bounceable()
    {
        //If the player isn't invincible and the player isn't touching the ground, then they are able to bounce on enemies
        if (!isInvincible && !myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        { ableToBounceOnEnemies = true; }
        else
        { ableToBounceOnEnemies = false; }
    }

    private void EnemyKilled()
    {
        //If the player has killed an enemy, make the player bounce & set the jumping animation
        if (killedAnEnemy)
        {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, enemyBounceHeight);
            //Setting the dashing to false was used to fix a bug where dashing into an axolotl then bouncing would keep the dash animation on
            playerAnimator.SetBool("Dashing", false);
            playerAnimator.SetBool("Jumping", true);
            killedAnEnemy = false;
        }
    }


    private void ClimbLadder()
    {
        //If the player is touching a ladder - do stuff
        if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            //Slow down the player when they touch a ladder
            runSpeed = 3f;
            //Similar to the axolotl bouncing, fixed a bug where the player would be stuck in the dash animation
            playerAnimator.SetBool("Dashing", false);
            //When the player presses the "vertical" button, add vertical velocity
            float controlThrow = CrossPlatformInputManager.GetAxis("Vertical"); // value is between -1 and +1
            Vector2 climbVelocity = new Vector2(playerRigidbody.velocity.x, controlThrow * climbSpeed);
            playerRigidbody.velocity = climbVelocity;

            //If the player is moving, play the climbing animation, if they aren't, play the wall sliding animation
            bool playerHasVerticalSpeed = Mathf.Abs(playerRigidbody.velocity.y) > Mathf.Epsilon;
            if(playerHasVerticalSpeed)
            {
                playerAnimator.SetBool("Climbing", true);
                playerAnimator.SetBool("Wall Slide", false);
            }
            else
            {
                playerAnimator.SetBool("Climbing", false);
                playerAnimator.SetBool("Wall Slide", true);
            }
            

           
            //Set the gravity to 0 while on the ladder
            //This might've actually been the reason for the infinite speed glitch I showed you
            playerRigidbody.gravityScale = 0;
        }
        else
        {
            //If the player isn't climbing, set the climbing animation to false
            playerAnimator.SetBool("Climbing", false);
            //Resets run speed after leaving ladder
            runSpeed = startRunSpeed;
            //Resets the gravity scale
            playerRigidbody.gravityScale = gravityScaleAtStart;
        }
    }

    private void WallSlide()
    {
        //If my body is touching the ground, but my feet aren't. That must mean I'm on a wall!
        //And the player needs to be pressing one of the directional buttons
        if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) &&
        !myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && CrossPlatformInputManager.GetButton("Horizontal"))
        { wallSliding = true; }
        else
        { wallSliding = false; }
        //If the player is wall sliding, set the animation and clamp my y velocity to the minimum of my wallsliding speed
        if (wallSliding == true)
        {
            playerAnimator.SetBool("Wall Slide", true);
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x,
            Mathf.Clamp(playerRigidbody.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        { playerAnimator.SetBool("Wall Slide", false); }
    }

    private void Fall()
    {
        //If my feet aren't touching the ground, and I'm not wall sliding, and I'm not touching a ladder or a platform, then I must be jumping
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && wallSliding == false
            && (!playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"))) && !myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Platform")))
        {
            playerAnimator.SetBool("Jumping", true);
            playerAnimator.SetBool("Running", false);
        }
        else { playerAnimator.SetBool("Jumping", false); }
    }

    //I added this to give the player a bit more control and smaller movement capabilities when tapping directional buttons
    //This helps with lining up tight jumps and making smaller adjustments when trying to dodge hazards
    private void Accelerate()
    {
        //If the player is dead, or if I'm touching a ladder, then don't accellerate
        if (!isAlive || playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"))) { return; }
        //If the player is moving left or right, gradually increase the speed until it's greater than the endRunSpeed (the maximum speed)
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (runSpeed < endRunSpeed)
            { runSpeed = startRunSpeed += 0.3f; }
        }
        else
        {
            //When the player stops moving, reset the run speed ready to accelerate again
            runSpeed = 2f;
            startRunSpeed = 2f;
        }
    }

    private void Run()
    {
        //If the player is locked, they can't run
        if(locked) { return; }
        //Apply a force on the horizontal axis at the speed set by runSpeed, using my velocity.y
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, playerRigidbody.velocity.y);
        playerRigidbody.velocity = playerVelocity;

        //Checks if the player has horizontal speed
        bool horizontalSpeed;
        if (horizontalSpeed = Mathf.Abs(playerRigidbody.velocity.x) > 0 && (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) || myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Platform"))))
        {
            //If the player's feet are touching the ground, start running animation
            
            playerAnimator.SetBool("Running", true); 
        }
        else
        { playerAnimator.SetBool("Running", false); }
    }

    private void Jump()
    {
        //If locked, you cant jump
        if (locked) { return; }

        /*myFeet is a collider placed at the feet of the player, the ! before myFeet means that the condition ISN'T true.
        { return; } stops the function from doing anything else after it.
        So - If my feet aren't touching the ground layer and they aren't touching the water, and they aren't touching a ladder
        then don't let me jump*/

        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) &&
            !playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Water")) &&
            !myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Platform"))
            || dashing)
        { return; }


        /* Using the cross platform input manager for this allows the jump button to be bound to whatever the player likes.
         This avoids the problem of hardcoding the spacebar - if the jump button isnt spacebar*/
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            //When the jump button is pressed, velocity is added to the vertical axis - adding velocity to any current velocity the player already has
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            playerRigidbody.velocity += jumpVelocityToAdd;
        }
    }

    private void WallJump()
    {
        //If the collider on the player's body isn't touching the wall, then reset the number of wall jumps to 0.
        if (!playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        { numberOfWallJumps = 0f; }
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        { numberOfWallJumps = 0f; }

        //If the body collider isn't touching the wall, or my feet are still on the ground, you can't wall jump.
        if (!playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }
        //When the jump button is pressed and the player hasn't wall jumped yet - add velocity to the y axis and add 1 to the number of wall jumps
        //In hindsight I don't know why I didn't just use a bool here - maybe it allowed for more adding more wall jumps?
        if (CrossPlatformInputManager.GetButtonDown("Jump") && numberOfWallJumps < 1f)
        {
            playerRigidbody.velocity = new Vector2(0f, wallJumpSpeed);
            numberOfWallJumps += 1;
        }
    }

    private void FlipSprite()
    {
        //If I have horizontal speed, then get the sign ( + or - ) of my x velocity and make that my scale (which flips the sprite)
        bool horizontalSpeed = Mathf.Abs(playerRigidbody.velocity.x) > 0;
        if (horizontalSpeed)
        { transform.localScale = new Vector2(Mathf.Sign(playerRigidbody.velocity.x), 1f); }
    }

    private void Dash()
    {
        //If the player hits the dash key and isn't touching the ground or a platform, AND isn't standing still AND is able to dash, then dash!
        if (CrossPlatformInputManager.GetButtonDown("Dash") && !myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && !myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Platform"))
            && playerRigidbody.velocity.x != 0 && ableToDash)
        {
            //If the player isn't pressing a directional key, don't do any of the rest
            if (Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.D) == false &&
                Input.GetKey(KeyCode.RightArrow) == false && Input.GetKey(KeyCode.LeftArrow) == false)
            { return; }

            //If the player has horizontal speed, then...
            bool horizontalSpeed = Mathf.Abs(playerRigidbody.velocity.x) > 0;
            if (horizontalSpeed)
            {
                //Set the dash direction to the direction I'm facing (-1 or 1)
                dashDirection = Mathf.Sign(playerRigidbody.velocity.x);
                //Stop my velocity completely
                playerRigidbody.velocity = Vector2.zero;
                //Set the current timer to the one at the start ( resets it )
                currentDashTimer = startDashTimer;
                //Update booleans
                dashing = true;
                ableToDash = false;
            }
        }

        //Once the booleans have been updated - do this stuff
        if (dashing)
        {
            //If the player presses any directional keys
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
            {
                //Get my transform.right property and multiply it by how fast I want to dash, then multiply by dash direction which is either 1 or -1 
                //Depending on which way I'm facing
                playerRigidbody.velocity = transform.right * dashSpeed * dashDirection;
                //Counts down the timer to 0
                currentDashTimer -= Time.deltaTime;
                //Set the animation of dashing
                playerAnimator.SetBool("Dashing", true);
                //When the timer reaches 0, stop dashing
                if (currentDashTimer <= 0)
                {
                    playerAnimator.SetBool("Dashing", false);
                    dashing = false;
                }
            }
        }
        //If at any time, the player's feet are touching the ground, reset being able to dash and make sure that the player is never dashing while on the ground
        //These last 2 lines were used to fix a bug where the player could dash twice by buffering the dash key until they were about to touch the ground
        //Then you could dash again.
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) || myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Platform"))
            //New to try and get extra dashes off of wall jumps
            || playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) || playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Platform")))
        {
            ableToDash = true;
            playerAnimator.SetBool("Dashing", false);
            dashing = false;
        }
    }
}