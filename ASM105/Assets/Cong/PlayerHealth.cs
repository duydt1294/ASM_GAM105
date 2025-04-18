using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private int currentHealth;

    private bool isInvincible = false; // Trạng thái bất tử
    [SerializeField] private float invincibilityDuration = 3f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return; // Nếu đang bất tử thì không nhận sát thương

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(BecomeTemporarilyInvincible());
        }
    }

    IEnumerator BecomeTemporarilyInvincible()
    {
        isInvincible = true;

        float elapsed = 0f;
        while (elapsed < invincibilityDuration)
        {
            // Nhấp nháy nhân vật (ẩn - hiện)
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.15f);

            elapsed += 0.3f;
        }

        isInvincible = false;
    }

    void Die()
    {
        Debug.Log("Player chết!");
        Destroy(gameObject);
    }
}
