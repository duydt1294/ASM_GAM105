using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDuy : MonoBehaviour
{
    // Các thành phần liên quan
    Animator animator;
    Rigidbody2D rb;
    AudioSource audioSource;
    // Các thông số điều khiển
    public float JumpForce = 10f;
    bool lat_mat;
    // Điều khiển chuyển động
    private float moveX;
    private Vector2 movement;
    // Kiểm tra mặt đất
    public LayerMask groundLayer;      // Lớp mặt đất
    public BoxCollider2D GroundCollider;
    public bool OnGround;              // xác định nhân vật đang trên mặt đất
    public Transform matdat;           // Vị trí kiểm tra mặt đất
    // Double Jump
    private int jumpCount = 0;         // Số lần đã nhảy
    private int maxJumps = 2;          // Tối đa nhảy 2 lần
    // Bắn đạn
    public GameObject bulletPrefab;    // Prefab đạn
    public Transform firePoint;        // Vị trí xuất phát đạn
    // Âm thanh
    public AudioClip jumpSound;
    public AudioClip shootSound;

    void Start()
    {
        // Gán component
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        OnGround = true;
        jumpCount = 0;
    }

    void Update()
    {
        // lật mặt trái/phải bằng A và D
        float trucX = 0f;
        if (Input.GetKey(KeyCode.A)) trucX = -1;
        if (Input.GetKey(KeyCode.D)) trucX = 1;

        // Đảo hướng nếu cần thiết
        if ((trucX < 0 && !lat_mat) || (trucX > 0 && lat_mat))
        {
            flip(); // Lật mặt nhân vật
        }

        // Bắn khi nhấn C
        if (Input.GetKeyDown(KeyCode.C))
        {
            animator.SetBool("Attack", true); // Bật animation tấn công
            Shoot(); // Bắn đạn
            if (shootSound != null)
                audioSource.PlayOneShot(shootSound); // Phát âm thanh bắn
        }
        else
        {
            animator.SetBool("Attack", false); // Tắt animation tấn công
        }

        // Đọc input trục ngang
        moveX = Input.GetAxisRaw("Horizontal");

        // Xử lý nhảy và double jump
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0); // Reset vận tốc Y trước khi nhảy
            rb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse); // Thêm lực đảy nhân vật để nhảy 
            jumpCount++;
            OnGround = false;

            if (jumpSound != null)
                audioSource.PlayOneShot(jumpSound); // Phát âm thanh nhảy
        }
        // Cập nhật animation nhảy
        animator.SetBool("Jump", !OnGround);
    }



    void flip()
    {
        lat_mat = !lat_mat; // Đảo trạng thái mặt
        Vector3 nv = transform.localScale;
        nv.x *= -1; // Đảo chiều nhân vật
        transform.localScale = nv;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Khi chạm đất → reset nhảy
        if (groundLayer == (1 << collision.gameObject.layer))
        {
            OnGround = true;
            jumpCount = 0;
            animator.SetBool("Jump", false);
        }

        // Nếu va chạm kẻ thù → chết
        if (collision.gameObject.tag.Equals("kethu"))
        {
            Destroy(gameObject); // Xoá nhân vật
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Rời khỏi mặt đất
        if (groundLayer == (1 << collision.gameObject.layer))
        {
            OnGround = false;
            animator.SetBool("Jump", true);
        }
    }

    void Shoot()
    {
        // Khởi tạo viên đạn
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        float direction = lat_mat ? -1f : 1f; // Xác định hướng bắn theo mặt
        bullet.GetComponent<bullet>().SetDirection(direction);
    }
}
