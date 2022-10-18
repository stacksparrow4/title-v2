using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign : MonoBehaviour
{
    [SerializeField] GameObject textPanel;
   
    //If the player enters the area around the sign, show the text panel with the text on it
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           textPanel.gameObject.SetActive(true);
        }
    }
    //If the player leaves the area, hide the text panel
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            textPanel.gameObject.SetActive(false);
        }
    }
}
