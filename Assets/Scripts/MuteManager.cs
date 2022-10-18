using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteManager : MonoBehaviour
{
    //This was never used in the end either but its here so may as well explain it.


    public static bool muted = false;
    private void Update()
    {
        //If the muted variable is true, then mute the audio, otherwise unmute
        if(muted)
        { GetComponent<AudioSource>().mute = true; }
        else { GetComponent<AudioSource>().mute = false; }
    }

    public void Mute()
    {
        //Every time the mute button is pressed, if its muted unmute, if its unmuted, mute.
        if (muted)
        { muted = false; }
        else { muted = true; }
    }
}
