using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossHealth : MonoBehaviour
{
    public float maxHealth = 100f;        // Máu tối đa của boss
    private float currentHealth;          // Máu hiện tại của boss
    public Slider healthBar;              // Slider UI cho thanh máu

    private bool isPlayerInArea = false;  // Kiểm tra xem player có trong khu vực không
    private bool hasHealthFilled = false; // Kiểm tra xem thanh máu đã đầy chưa

    private void Start()
    {
        currentHealth = 0f;               // Khởi tạo máu hiện tại là 0
        healthBar.gameObject.SetActive(false); // Ẩn thanh máu khi bắt đầu
    }

    // Hàm giảm máu boss
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
            currentHealth = 0;

        UpdateHealthBar(); // Cập nhật thanh máu sau khi giảm

        // Nếu máu boss bằng 0, ẩn thanh máu và tắt boss
        if (currentHealth <= 0)
        {
            healthBar.gameObject.SetActive(false); // Ẩn thanh máu khi boss chết
            gameObject.SetActive(false); // Tắt hoàn toàn boss khi chết
        }
    }

    // Hàm cập nhật thanh máu UI
    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth; // Cập nhật giá trị của Slider
        }
    }

    // Hàm hồi phục máu (nếu có thể)
    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UpdateHealthBar(); // Cập nhật thanh máu sau khi hồi phục
    }

    // Kiểm tra khi player vào và rời khỏi khu vực
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Kiểm tra khi player vào khu vực
        {
            isPlayerInArea = true;
            healthBar.gameObject.SetActive(true); // Hiển thị thanh máu khi player vào

            // Kiểm tra xem hiệu ứng điền thanh máu đã được thực hiện chưa
            if (!hasHealthFilled)
            {
                StartCoroutine(ShowHealthBarAndFill()); // Bắt đầu hiệu ứng dần dần
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Kiểm tra khi player rời khỏi khu vực
        {
            isPlayerInArea = false;
            // Ẩn thanh máu khi player rời khỏi khu vực, nhưng không ảnh hưởng đến trạng thái điền máu
            healthBar.gameObject.SetActive(false);
        }
    }

    // Coroutine để tạo hiệu ứng thanh máu từ từ xuất hiện và tăng lên
    private IEnumerator ShowHealthBarAndFill()
    {
        hasHealthFilled = true; // Đánh dấu là đã thực hiện hiệu ứng điền thanh máu

        // Làm cho thanh máu từ từ xuất hiện
        float targetHealth = maxHealth;
        float fillSpeed = 100f; // Tốc độ điền thanh máu

        // Tăng dần giá trị thanh máu từ 0 đến maxHealth
        while (currentHealth < targetHealth)
        {
            currentHealth += Time.deltaTime * fillSpeed; // Dùng Time.deltaTime để làm cho nó mượt mà
            if (currentHealth > targetHealth) // Tránh vượt quá maxHealth
                currentHealth = targetHealth;

            UpdateHealthBar(); // Cập nhật thanh máu
            yield return null; // Chờ đợi trong mỗi frame
        }
    }
}
