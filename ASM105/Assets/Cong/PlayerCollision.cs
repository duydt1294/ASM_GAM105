using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public int damage = 1; // Lượng sát thương mà kẻ thù gây ra

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra nếu player va chạm với enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Lấy component PlayerHealth từ chính Player (tức là gameObject đang gắn script này)
            PlayerHealth playerHealth = GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // Trừ máu player
            }
            else
            {
                Debug.LogWarning("Không tìm thấy PlayerHealth trên Player.");
            }
        }
    }
}
