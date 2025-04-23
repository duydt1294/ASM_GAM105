using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Logic chết của người chơi (Ví dụ: Hiện thông báo hoặc reset scene)
        Debug.Log("Player died");
    }
}
