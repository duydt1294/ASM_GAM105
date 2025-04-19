using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    // Tốc độ di chuyển của Player
    public float moveSpeed = 5f;

    // Biến để lưu trữ hướng di chuyển
    private Vector2 moveDirection;

    // SerializedField để dễ dàng chỉnh sửa vị trí dịch chuyển trong Inspector
    [SerializeField] private Vector3 portalDestination = new Vector3(357.58f, -22.65f, 0f);

    void Update()
    {
        // Lấy input từ bàn phím
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D hoặc mũi tên trái/phải
        float moveY = Input.GetAxisRaw("Vertical");   // W/S hoặc mũi tên lên/xuống

        // Cập nhật hướng di chuyển
        moveDirection = new Vector2(moveX, moveY).normalized;

        // Di chuyển PlayerTest
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    // Hàm xử lý va chạm với các collider 2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra nếu đối tượng va chạm có Tag "Portal"
        if (collision.gameObject.CompareTag("Portal"))
        {
            // Di chuyển nhân vật đến vị trí mới từ SerializedField
            transform.position = portalDestination;
        }
    }
}
