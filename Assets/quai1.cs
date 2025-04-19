using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quai1 : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Thêm animation chết hoặc logic xử lý chết ở đây
        Debug.Log("Hunter đã chết!");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hunter"))
        {
            quai1 hunter = other.GetComponent<quai1>();
            if (hunter != null)
            {
                hunter.TakeDamage(10); // Gây 10 sát thương
            }
        }
    }
}