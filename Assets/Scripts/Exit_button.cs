using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButtonHandler : MonoBehaviour
{
    // Method to load a specific scene
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene("Loading");
    }

    // Optional: Method to quit the application
    public void QuitGame()
    {
        Application.Quit();
    }
}