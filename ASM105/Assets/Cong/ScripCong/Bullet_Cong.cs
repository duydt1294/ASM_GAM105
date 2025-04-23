using UnityEngine;

public class Bullet_Cong : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private float direction;
    private bool hit;
    private CapsuleCollider2D CapsuleCollider;

    private void Awake()
    {
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
    }
    private void Update()
    {
        if (hit) return;
        float movementSpeed = Time.deltaTime * speed * direction;
        transform.Translate(movementSpeed, 0, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Nếu va chạm với kẻ thù
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //Destroy(collision.gameObject); // Xóa kẻ thù
            Destroy(gameObject); // Xóa viên đạn
            return;
        }
    }
    public void SetDirection(float _direction)
    {
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        CapsuleCollider.enabled = true;

        // Cập nhật hướng scale viên đạn
        Vector3 localScale = transform.localScale;
        if (Mathf.Sign(localScale.x) != Mathf.Sign(_direction))
        {
            localScale.x *= -1;
        }
        transform.localScale = localScale;
        Destroy(gameObject, 1f);
    }
}
