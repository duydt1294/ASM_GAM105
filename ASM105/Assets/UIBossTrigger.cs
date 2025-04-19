using UnityEngine;
using System.Collections;

public class UIBossTrigger : MonoBehaviour
{
    public GameObject uiBoss;            // Gán trong Inspector
    public float fadeDuration = 2f;      // Thời gian fade in và fade out
    public float displayTime = 1.5f;     // Thời gian UI hiển thị trước khi biến mất

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(FadeUIRoutine());
        }
    }

    IEnumerator FadeUIRoutine()
    {
        uiBoss.SetActive(true);

        CanvasGroup canvasGroup = uiBoss.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = uiBoss.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0;

        // Fade in
        float t = 0f;
        while (t < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;

        // Wait
        yield return new WaitForSeconds(displayTime);

        // Fade out
        t = 0f;
        while (t < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0;

        uiBoss.SetActive(false);
    }
}
