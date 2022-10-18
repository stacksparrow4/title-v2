using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFountain : MonoBehaviour
{
    [SerializeField] Sprite clearSprite;
    [SerializeField] Sprite corruptSprite;
    [SerializeField] GameObject particles;

    void Update()
    {
        if(PlayerPrefs.HasKey("bossClear"))
        {
            if(PlayerPrefs.GetInt("bossClear") == 1)
            {
                particles.gameObject.SetActive(false);
                GetComponent<SpriteRenderer>().sprite = clearSprite;
            }
        }
        else 
        {            
            particles.gameObject.SetActive(true);
            GetComponent<SpriteRenderer>().sprite = corruptSprite;
        }
    }
}
