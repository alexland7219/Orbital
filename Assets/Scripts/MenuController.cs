using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject audioManager;

    public void StartGame()
    {
        Destroy(audioManager.gameObject);
        SceneManager.LoadScene(1);
    }
    public void OpenCredits()
    {
        SceneManager.LoadScene(2);
    }

    public void OpenInstructions(){
        SceneManager.LoadScene(3);

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
