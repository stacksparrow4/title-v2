using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelSelect : MonoBehaviour
{
    // Each public method is connected to one of the buttons on the level select panel
    // Clicking the corresponding button, will load the level you clicked on.

    public void Level1()
    { SceneManager.LoadScene(1); }

    public void Level2()
    { SceneManager.LoadScene(2); }

    public void Level3()
    { SceneManager.LoadScene(3); }

    public void Level4()
    { SceneManager.LoadScene(4); }

    public void Level5()
    { SceneManager.LoadScene(5); }

    public void Level6()
    { SceneManager.LoadScene(6); }

    public void Level7()
    { SceneManager.LoadScene(7); }

    public void LevelBoss()
    { SceneManager.LoadScene(8); }
}
