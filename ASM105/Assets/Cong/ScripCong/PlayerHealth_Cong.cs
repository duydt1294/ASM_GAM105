using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Thêm dòng này

public class PlayerHealth_Cong : MonoBehaviour
{
    [Header("Sức khỏe")]
    [SerializeField] private int maxHealth = 5; // Máu tối đa của player
    [SerializeField] private int currentHealth; // Máu hiện tại của player
                                    
    private bool isInvincible = false; // Trạng thái bất tử
    [SerializeField] private float invincibilityDuration = 3f; // Thời gian bất tử

    private SpriteRenderer spriteRenderer; // Sprite của player
    private AudioSource audioSource; // Âm thanh của player

    [Header("Âm thanh")]
    [SerializeField] private AudioClip takeDamageSound; // Âm thanh khi player chịu sát thương
    [SerializeField] private AudioClip deathSound; // Âm thanh khi player chết
    public int damage = 1; // Lượng sát thương từ kẻ thù

    [Header("Load Scene")]
    [SerializeField] private string sceneToLoad; // Tên scene cần load

    void Start()
    {
        currentHealth = maxHealth; // Khởi tạo máu player bằng máu tối đa
        spriteRenderer = GetComponent<SpriteRenderer>(); // Lấy component SpriteRenderer của player
        audioSource = GetComponent<AudioSource>(); // Lấy component AudioSource của player
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra nếu player va chạm với enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player collided with Enemy!"); // Debug log
            TakeDamage(damage); // Trừ máu player
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return; // Nếu đang trong trạng thái bất tử thì không trừ máu

        currentHealth -= damage; // Trừ máu player

        // Phát âm thanh khi chịu sát thương
        PlayTakeDamageSound();

        // Kiểm tra nếu máu còn lại <= 0 thì player chết
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Nếu còn máu, bắt đầu trạng thái bất tử và nhấp nháy sprite
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    private void PlayTakeDamageSound()
    {
        if (takeDamageSound != null)
        {
            audioSource.PlayOneShot(takeDamageSound); // Phát âm thanh chịu sát thương
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true; // Đặt trạng thái bất tử

        float elapsed = 0f;
        while (elapsed < invincibilityDuration)
        {
            // Nhấp nháy sprite của player
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f); // Thời gian nhấp nháy
            elapsed += 0.1f;
        }

        // Sau khi hết thời gian bất tử, tắt trạng thái bất tử
        spriteRenderer.enabled = true; // Đảm bảo sprite không bị ẩn
        isInvincible = false;
    }

    private void Die()
    {
        StartCoroutine(DieCoroutine()); // Gọi coroutine để xử lý cái chết
    }

    private IEnumerator DieCoroutine()
    {
        // Phát âm thanh chết
        if (deathSound != null)
        {
            audioSource.PlayOneShot(deathSound); // Phát âm thanh khi player chết
        }

        Debug.Log("Player has died!"); // Log khi player chết

        // Chờ 3 giây trước khi load scene khác
        yield return new WaitForSeconds(3f);

        // Load scene mới
        SceneManager.LoadScene(sceneToLoad); // Load scene theo tên đã chỉ định
    }
}