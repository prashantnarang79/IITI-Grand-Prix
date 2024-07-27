using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ButtonStart()
    {
        SceneManager.LoadScene("Loading");
    }

    public void ButtonCredit()
    {
        SceneManager.LoadScene("Credits");
    }

    public void ButtonQuit()
    { 
        Application.Quit();
    }
}
