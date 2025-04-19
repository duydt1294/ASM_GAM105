using UnityEngine;

public class MeteorFly : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    public float fallSpeed = 5f;  // Tốc độ rơi theo trục Y
    public float moveSpeed = 3f;  // Tốc độ di chuyển theo trục X

    public GameObject explosionEffect; // Hiệu ứng nổ (Prefab nổ)

    void Start()
    {
        // Lấy tham chiếu tới Animator và Rigidbody2D
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Thiên thạch di chuyển theo hướng xéo (di chuyển cùng lúc theo cả trục X và Y)
        rb.velocity = new Vector2(moveSpeed, -fallSpeed);  // Tạo chuyển động xéo (phải và xuống)
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra Tag của đối tượng va chạm
        if (other.CompareTag("Dat") || other.CompareTag("Player"))
        {
            // Khi thiên thạch va chạm với đối tượng có Tag "Dat" hoặc "Player", kích hoạt animation nổ
            animator.SetTrigger("MeoteorCoi");

            // Kích hoạt hiệu ứng nổ (nếu có)
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);  // Spawn hiệu ứng nổ tại vị trí thiên thạch
            }

            // Hủy thiên thạch sau khi va chạm và phát nổ
            Destroy(gameObject, 0.1f);  // Hủy sau 0.5 giây để cho hiệu ứng nổ có thời gian phát ra
        }
    }
}
