using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("ThisGame");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
