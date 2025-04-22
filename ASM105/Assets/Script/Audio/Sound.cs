using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverAndClickSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    private SoundManager soundManager;  // Tham chiếu đến SoundManager

    void Start()
    {
        // Tìm SoundManager trong scene
        soundManager = FindObjectOfType<SoundManager>();
    }

    // Phát âm thanh khi con trỏ chuột di chuyển vào nút
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (soundManager != null)
        {
            soundManager.PlayHoverSound();
        }
    }

    // Phát âm thanh khi nút bị nhấn
    public void OnPointerClick(PointerEventData eventData)
    {
        if (soundManager != null)
        {
            soundManager.PlayClickSound();
        }
    }
}
