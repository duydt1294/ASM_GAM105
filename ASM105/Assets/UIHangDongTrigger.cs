using UnityEngine;
using System.Collections;

public class UIHangDongTrigger : MonoBehaviour
{
    public GameObject uiHangDong; // Gán trong Inspector
    public float fadeDuration = 2f; // Thời gian fade in/out

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasTriggered && collision.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(ShowAndFadeUI());
        }
    }

    IEnumerator ShowAndFadeUI()
    {
        uiHangDong.SetActive(true);

        CanvasGroup canvasGroup = uiHangDong.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = uiHangDong.AddComponent<CanvasGroup>();
        }

        // Fade in
        float t = 0;
        while (t < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;

        // Wait a moment before fading out
        yield return new WaitForSeconds(1f);

        // Fade out
        t = 0;
        while (t < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0;

        uiHangDong.SetActive(false);
    }
}
