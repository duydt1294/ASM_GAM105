using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;


public class OptionMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;
    public float savedVolume;
    public void Start()
    {
        // Lấy âm lượng lưu trước đó (mặc định là 0 nếu chưa lưu)
        savedVolume = PlayerPrefs.GetFloat("VolumeLevel", 0f);
        audioMixer.SetFloat("volume", savedVolume);
        volumeSlider.value = savedVolume; // Cập nhật slider theo giá trị đã lưu
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("VolumeLevel", volume); // lưu lại giá trị khi thay đổi âm lượng
    }
}
