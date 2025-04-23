using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SoundManager : MonoBehaviour
{
    public AudioClip hoverSound;  // Âm thanh khi chuột di chuyển vào nút
    public AudioClip clickSound;  // Âm thanh khi nút bị nhấn
    private AudioSource audioSource;  // Đối tượng AudioSource để phát âm thanh

    void Start()
    {
        // Tạo một AudioSource cho SoundManager để phát âm thanh
        audioSource = GetComponent<AudioSource>();
    }

    // Phát âm thanh khi con trỏ chuột di chuyển vào nút
    public void PlayHoverSound()
    {
        if (hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    // Phát âm thanh khi nút bị nhấn
    public void PlayClickSound()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
