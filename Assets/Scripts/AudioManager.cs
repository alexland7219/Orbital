using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    
    public AudioSource menuTheme;   // 0
    public AudioSource bgTheme;     // 1
    public AudioSource golemStep;   // 2
    public AudioSource golemDamage; // 3
    public AudioSource golemDeath;  // 4
    public AudioSource golemPunch;  // 5
    public AudioSource golemSwing;  // 6
    public AudioSource winMusic;    // 7
    public AudioSource bossMusic;   // 8
    public AudioSource death;   // 9
    public AudioSource rockSmash;     // 10
    public AudioSource portalRing;   // 11
    public AudioSource pain;   // 12
    public AudioSource reload;   // 13

    public AudioSource[] audioSources;

    void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSources = new AudioSource[14];
            audioSources[0] = menuTheme;
            audioSources[1] = bgTheme;
            audioSources[2] = golemStep;
            audioSources[3] = golemDamage;
            audioSources[4] = golemDeath;
            audioSources[5] = golemPunch;
            audioSources[6] = golemSwing;
            audioSources[7] = winMusic;
            audioSources[8] = bossMusic;
            audioSources[9] = death;
            audioSources[10] = rockSmash;
            audioSources[11] = portalRing;
            audioSources[12] = pain;
            audioSources[13] = reload;


            audioSources[0].Play();

        } 
        else {
            audioSources = new AudioSource[14];
            audioSources[0] = menuTheme;
            audioSources[1] = bgTheme;
            audioSources[2] = golemStep;
            audioSources[3] = golemDamage;
            audioSources[4] = golemDeath;
            audioSources[5] = golemPunch;
            audioSources[6] = golemSwing;
            audioSources[7] = winMusic;
            audioSources[8] = bossMusic;
            audioSources[9] = death;
            audioSources[10] = rockSmash;
            audioSources[11] = portalRing;
            audioSources[12] = pain;
            audioSources[13] = reload;

        }
    }

    public void PlayAtIndex(int i)
    {
        audioSources[i].Play();
    }
    public void StopAtIndex(int i)
    {
        audioSources[i].Stop();
    }
    public void PauseAtIndex(int i)
    {
        audioSources[i].Pause();
    }
    public void ResumeAtIndex(int i)
    {
        audioSources[i].UnPause();
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


}