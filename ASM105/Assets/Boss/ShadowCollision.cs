using UnityEngine;

public class ShadowCollision : MonoBehaviour
{
    public float damage; // Sát thương
    public float slowDuration; // Thời gian làm chậm
    public float slowAmount; // Tỷ lệ làm chậm

    //Khi bóng tối va chạm với đối tượng khác
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Gây sát thương cho người chơi
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            // Áp dụng hiệu ứng làm chậm cho người chơi
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.ApplySlow(slowAmount, slowDuration);
            }

            // Hủy bóng tối sau khi va chạm
            Destroy(gameObject);
        }
    }
}
