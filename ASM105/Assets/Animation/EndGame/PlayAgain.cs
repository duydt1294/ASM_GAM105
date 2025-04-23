using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgain : MonoBehaviour
{
    [SerializeField] GameObject playAgainButton;
    void Start()
    {
        playAgainButton.SetActive(false);
        StartCoroutine(DelayButton());
    }
    IEnumerator DelayButton()
    {
        yield return new WaitForSeconds(36f);
        playAgainButton.SetActive(true);
    }
    public void OnPlayAgainButtonClicked()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
