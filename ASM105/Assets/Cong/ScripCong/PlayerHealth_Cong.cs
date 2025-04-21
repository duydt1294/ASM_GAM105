using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth_Cong : MonoBehaviour
{
    [Header("Sức khỏe")]
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private int currentHealth;

    private bool isInvincible = false; // Trạng thái bất tử
    [SerializeField] private float invincibilityDuration = 3f;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    [Header("Âm thanh")]
    [SerializeField] private AudioClip takeDamageSound; // Âm thanh chịu sát thương
    [SerializeField] private AudioClip deathSound; // Âm thanh chết
    public int damage = 1; // Lượng sát thương mà kẻ thù gây ra

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
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
        if (isInvincible) return;

        currentHealth -= damage;

        // Phát âm thanh chịu sát thương
        if (takeDamageSound != null)
        {
            audioSource.PlayOneShot(takeDamageSound);
        }

        StartCoroutine(InvincibilityCoroutine());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

        float elapsed = 0f;
        while (elapsed < invincibilityDuration)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.2f;
        }

        isInvincible = false;
    }

    private void Die()
    {
        // Phát âm thanh chết
        if (deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        Debug.Log("Player has died!");

        // Ẩn player hoặc vô hiệu hóa điều khiển
        gameObject.SetActive(false);
    }
}
