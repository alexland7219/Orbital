using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("HELLO");

        SceneManager.LoadScene(1);
    }
    public void OpenCredits()
    {
        // Implement settings menu logic here.
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
