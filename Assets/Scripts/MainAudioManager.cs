using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAudioManager : MonoBehaviour
{
    public AudioSource bgTheme;
    public AudioSource golemStep;
    public AudioSource golemDamage;
    public AudioSource golemDeath;
    public AudioSource golemPunch;
    public AudioSource golemSwing;
    public AudioSource winMusic;
    //public AudioSource golemHit;

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

    public void PlayGolemStepSound(){
        golemStep.pitch = 1.0f;
        golemStep.volume = 0.6f;
        golemStep.Play();
    }

    public void PlayGolemFallSound(){
        golemStep.pitch = 0.5f;
        golemStep.volume = 1.0f;
        golemStep.Play();
    }

    public void PlayGolemDamageSound() { golemDamage.Play(); }

    public void PlayGolemDeathSound() {
        bgTheme.Pause();
        golemDeath.Play(); 
    }

    public void PlayWinMusic() { winMusic.Play(); }

    public void PlayGolemPunchSound() { golemPunch.Play(); }

    public void PlayGolemSwingSound() { golemSwing.Play(); }

}
