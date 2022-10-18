using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    // This script is something I copied from somewhere, and I honestly couldn't tell you where...
    //It's similar if not the same as my GlobalObject script which basically just creates persistent
    //background music throughout the scenes and carries over if you die.

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("MusicTag");
        if (objs.Length > 1)
        { Destroy(this.gameObject); }
        else
        { DontDestroyOnLoad(this.gameObject); }
    }
}
