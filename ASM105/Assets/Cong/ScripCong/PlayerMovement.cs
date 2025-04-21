using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Di chuyển")]
    [SerializeField] private float moveSpeed = 5f; // Tốc độ di chuyển
    [SerializeField] private float jumpForce = 12f; // Lực nhảy
    [SerializeField] private float TimeOutWall;

    [Header("Kiểm tra va chạm")]
    [SerializeField] private LayerMask groundLayer; // Layer cho mặt đất
    [SerializeField] private LayerMask wallLayer; // Layer cho tường

    private Rigidbody2D rb; // Rigidbody2D của nhân vật
    private Animator animator; // Animator để điều khiển animation
    private SpriteRenderer sprite; // SpriteRenderer để thay đổi hình ảnh

    private float horizontal; // Giá trị di chuyển ngang
    private bool isGrounded; // Kiểm tra có đứng trên mặt đất không
    private bool isWallSliding; // Kiểm tra có đang bám tường không
    private bool isOutWallSliding; // Kiểm tra nhảy ra khỏi tường
    private bool isTouchingWall; // Kiểm tra có chạm vào tường không
    private bool canJump; // Kiểm tra có thể nhảy không

    // Attack-related variables
    private AudioSource audioSource;
    [SerializeField] private AudioClip jumpSound; // Âm thanh nhảy
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip runSound; // Âm thanh chạy
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private int damage = 1; // Lượng sát thương từ kẻ thù

    private bool isRunning; // Kiểm tra có đang chạy không

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Lấy Rigidbody2D
        animator = GetComponent<Animator>(); // Lấy Animator
        sprite = GetComponent<SpriteRenderer>(); // Lấy SpriteRenderer
        audioSource = GetComponent<AudioSource>(); // Lấy AudioSource
    }

    void Update()
    {
        // Kiểm tra tấn công
        if (Input.GetKeyDown(KeyCode.R))
        {
            Attack();
            return; // Ngăn không cho tiếp tục xử lý các logic khác
        }

        horizontal = Input.GetAxisRaw("Horizontal"); // Lấy giá trị di chuyển ngang

        // Kiểm tra bám tường chỉ khi nhân vật chạm vào tường và không đứng trên mặt đất
        if (isTouchingWall && !isGrounded && rb.velocity.y <= 0)
        {
            isWallSliding = true; // Bắt đầu bám tường ngay lập tức khi chạm vào tường

            // Quay hình ảnh vào tường
            if (horizontal > 0)
            {
                sprite.flipX = true;
            }
            else if (horizontal < 0)
            {
                sprite.flipX = false;
            }
        }
        else
        {
            isWallSliding = false;
        }

        // Kiểm tra nhảy ra khỏi tường khi nhấn nút nhảy
        if (Input.GetButtonDown("Jump") && canJump)
        {
            if (isWallSliding)
            {
                isOutWallSliding = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isWallSliding = false;

                // Phát âm thanh nhảy khi nhảy ra khỏi tường
                if (jumpSound != null)
                {
                    audioSource.PlayOneShot(jumpSound);
                }
                else
                {
                    Debug.LogWarning("jumpSound is not assigned!");
                }

                StartCoroutine(TransitionToOutWallSliding());
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animator.Play("Jump");

                // Phát âm thanh nhảy khi nhảy bình thường
                if (jumpSound != null)
                {
                    audioSource.PlayOneShot(jumpSound);
                }
                else
                {
                    Debug.LogWarning("jumpSound is not assigned!");
                }
            }
        }

        // Xoay sprite theo hướng
        if (horizontal > 0) sprite.flipX = false;
        else if (horizontal < 0) sprite.flipX = true;

        // Gửi trạng thái vào Animator
        UpdateAnimations();

        // Phát âm thanh chạy
        if (Mathf.Abs(horizontal) > 0 && isGrounded)
        {
            if (!isRunning)
            {
                audioSource.loop = true; // Bật chế độ lặp
                audioSource.clip = runSound; // Gán âm thanh chạy
                audioSource.Play(); // Phát âm thanh
                isRunning = true; // Đánh dấu là đang chạy
            }
        }
        else
        {
            if (isRunning)
            {
                audioSource.Stop(); // Dừng âm thanh khi không còn chạy
                isRunning = false; // Đánh dấu là không còn chạy
            }
        }
    }

    void FixedUpdate()
    {
        if (!isWallSliding)
        {
            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        }

        if (isWallSliding)
        {
            rb.velocity = new Vector2(horizontal * moveSpeed, 0);
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 1;
        }
    }

    void UpdateAnimations()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;

        if (isWallSliding)
        {
            animator.Play(sprite.flipX ? "WallSlide" : "WallSlide");
        }
        else if (isOutWallSliding)
        {
            animator.Play("OutWallSlide");
        }
        else if (rb.velocity.y != 0)
        {
            animator.Play("Jump");
        }
        else if (Mathf.Abs(horizontal) > 0 && isGrounded)
        {
            animator.Play("Run");
        }
        else
        {
            animator.Play("Idle");
        }
    }
    void Attack()
    {
        animator.Play("Attack");

        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not assigned!");
            return;
        }

        if (shootSound == null)
        {
            Debug.LogError("shootSound is not assigned!");
            return;
        }

        audioSource.PlayOneShot(shootSound);
        Shoot();
        StartCoroutine(ResetToIdleAfterAttack());
    }

    private IEnumerator ResetToIdleAfterAttack()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.Play("Idle");
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        float direction = sprite.flipX ? -1f : 1f;
        bullet.GetComponent<Bullet>().SetDirection(direction);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra nếu player va chạm với enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player collided with Enemy!");
            GetComponent<PlayerHealth_Cong>().TakeDamage(damage); // Gọi hàm TakeDamage
        }

        // Cập nhật trạng thái va chạm với groundLayer và wallLayer
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
            canJump = true;
        }

        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            isTouchingWall = true;
            canJump = true;
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            canJump = true;
        }

        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            isTouchingWall = true;
            canJump = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = false;
            canJump = false;
        }

        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            isTouchingWall = false;
            canJump = false;
        }
    }

    private IEnumerator TransitionToOutWallSliding()
    {
        yield return new WaitForSeconds(0.1f);
        animator.Play("OutWallSlide");
        yield return new WaitForSeconds(TimeOutWall);
        animator.Play("Jump");
        isOutWallSliding = false;
    }
}