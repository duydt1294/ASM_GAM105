using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;    

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    private OptionMenu optionMenu;
    private float originalvolume;
    void Start()
    {
        optionMenu = FindObjectOfType<OptionMenu>();
        optionMenu.audioMixer.GetFloat("volume", out originalvolume);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        GameIsPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        optionMenu.audioMixer.SetFloat("volume", originalvolume);
    }
    public void Pause()
    {
        GameIsPaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        float newVolume = originalvolume - 10f;
        optionMenu.audioMixer.SetFloat("volume", newVolume);
    }
    public void MainMenu() 
    {
        SceneManager.LoadScene("StartMenu");
        Time.timeScale = 1f;
    }
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
