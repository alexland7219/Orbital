using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    
    public AudioSource menuTheme;

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    public void stopMenuMusic()
    {
        menuTheme.Stop();
    }

}