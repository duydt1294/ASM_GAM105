using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth_Cong : MonoBehaviour
{
    [Header("Sức khỏe")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private Slider thanhMau;

    private bool isInvincible = false;
    [SerializeField] private float invincibilityDuration = 3f;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    [Header("Âm thanh")]
    [SerializeField] private AudioClip takeDamageSound;
    [SerializeField] private AudioClip deathSound;
    public int damage = 5;

    [Header("Load Scene")]
    [SerializeField] private string sceneToLoad = "EndGame";

    private bool isDead = false; // Thêm biến trạng thái

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return; // Nếu đã chết, không xử lý va chạm

        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return; // Nếu đã chết, không xử lý va chạm

        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(damage);
        }
        if (collision.gameObject.CompareTag("Water"))
        {
            TakeDamage(100);
        }
        if (collision.gameObject.CompareTag("Heal"))
        {
            currentHealth = 100;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || isDead) return; // Nếu đang bất tử hoặc đã chết thì không trừ máu

        currentHealth -= damage;
        Debug.Log(currentHealth);

        PlayTakeDamageSound();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    private void PlayTakeDamageSound()
    {
        if (takeDamageSound != null)
        {
            audioSource.PlayOneShot(takeDamageSound);
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

        float elapsed = 0f;
        while (elapsed < invincibilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        spriteRenderer.enabled = true;
        isInvincible = false;
    }

    private void Die()
    {
        if (isDead) return; // Nếu đã chết rồi thì không làm gì thêm
        isDead = true; // Đặt trạng thái đã chết
        StartCoroutine(DieCoroutine()); // Gọi coroutine để xử lý cái chết
    }

    private IEnumerator DieCoroutine()
    {
        // Phát âm thanh chết
        if (deathSound != null)
        {
            audioSource.PlayOneShot(deathSound); // Phát âm thanh khi player chết
        }

        Debug.Log("Player has died!");

        // Tắt tất cả các script khác của player
        DisablePlayerScripts();

        // Chờ âm thanh chết phát xong trước khi load scene khác
        yield return new WaitForSeconds(deathSound.length);

        // Load scene mới
<<<<<<< HEAD
        SceneManager.LoadScene(sceneToLoad);
=======
        SceneManager.LoadScene("GameOver");
>>>>>>> merge_dat
    }

    private void DisablePlayerScripts()
    {
        // Tìm tất cả các script trên gameObject này và tắt chúng
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            script.enabled = false; // Tắt từng script
        }
    }

    private void Update()
    {
        if (isDead) return; // Nếu đã chết, không thực hiện cập nhật

        thanhMau.value = currentHealth; // Cập nhật thanh máu
    }
}