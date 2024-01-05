using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAudioManager : MonoBehaviour
{
    public AudioSource bgTheme;
    public AudioSource shoot;

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

    public void PlayShootSound(){
        shoot.Play();
    }

}
