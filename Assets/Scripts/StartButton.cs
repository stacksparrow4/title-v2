using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    //References
    [SerializeField] Image blackImage;
    [SerializeField] Animator startButtonAnimator;
    [SerializeField] Animator optionsButtonAnimator;
    [SerializeField] Animator exitButtonAnimator;
    [SerializeField] Animator startButtonAnimatorTXT;
    [SerializeField] Animator optionsButtonAnimatorTXT;
    [SerializeField] Animator exitButtonAnimatorTXT;
    [SerializeField] Animator TitleAnimatorTXT;
    [SerializeField] Animator SubtitleAnimatorTXT;
    //Called when the play button is pressed
    public void LoadNextScene()
    {
        StartCoroutine(StartDelay());
    }

    private IEnumerator StartDelay()
    {
        //Waits for a bit after the play button is pressed
        yield return new WaitForSecondsRealtime(.5f);
        //Makes all objects on the main menu fade out
        blackImage.GetComponent<Animator>().SetBool("Fade", true);
        startButtonAnimator.SetBool("Fade", true);
        optionsButtonAnimator.SetBool("Fade", true);
        exitButtonAnimator.SetBool("Fade", true);
        startButtonAnimatorTXT.SetBool("Fade", true);
        optionsButtonAnimatorTXT.SetBool("Fade", true);
        exitButtonAnimatorTXT.SetBool("Fade", true);
        TitleAnimatorTXT.SetBool("Fade", true);
        SubtitleAnimatorTXT.SetBool("Fade", true);
        //Waits for a second
        yield return new WaitForSecondsRealtime(1f);

        //Loads the next level (level 1)
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    //Called when the options button is pressed - updates the global variable to be used in OpenMenu.cs
   public void MenuOpen()
    {
        OpenMenu.menuIsOpen = true;
    }
    //Called when the play button is pressed - updates the global variable to be used in OpenMenu.cs
    public void MenuClose()
    {
        OpenMenu.menuIsOpen = false;
    }

    public void PlayMenuOpen()
    {
        OpenMenu.playMenuIsOpen = true;
    }



}
