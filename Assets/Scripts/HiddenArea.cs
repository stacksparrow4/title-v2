using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HiddenArea : MonoBehaviour
{
    //References
    [SerializeField] Player player;
    CompositeCollider2D areaCollider;
    [SerializeField] Tilemap hiddenArea;

    //Checking if the player is colliding with the hidden area collider
    bool isColliding = false;

    // Start is called before the first frame update
    private void Start()
    {
        //Retrieves references from the gameobject and puts them into a variable
        areaCollider = GetComponent<CompositeCollider2D>();        
    }

    // Update is called once per frame
    private void Update()
    {
        PlayerCollision();
        //If the player is colliding with a secret area, set the "colour" of the tilemap to slightly transparent, else - set it back to normal
        if (isColliding)
        {
            hiddenArea.color = new Color(1f, 1f, 1f, 0.4f);
        }
        else
        {
            hiddenArea.color = new Color(1f, 1f, 1f, 1f);

        }
    }

    private void PlayerCollision()
    {        
        //If the area of the hidden area intersects with the area of the player's collider, then update the variable
        if (areaCollider.bounds.Intersects(player.GetComponent<CapsuleCollider2D>().bounds))
        {
            isColliding = true;
        }
        else
        {
            isColliding = false;
        }
    }
}
