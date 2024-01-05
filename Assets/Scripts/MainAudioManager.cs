using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAudioManager : MonoBehaviour
{
    public AudioSource bgTheme;

    void Start(){
        bgTheme.loop = true;
        bgTheme.Play();
    }

    public void PauseTheme(){
        bgTheme.Pause();
    }

    public void ResumeTheme(){
        bgTheme.UnPause();
    }

}
