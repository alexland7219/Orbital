using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonMenu : MonoBehaviour
{

    public void Back(){
        SceneManager.LoadScene(0);
    }

}
