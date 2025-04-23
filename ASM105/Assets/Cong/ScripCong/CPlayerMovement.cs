using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerMovement : MonoBehaviour
{
    [Header("Di chuyển")]
    [SerializeField] private float moveSpeed = 5f; // Tốc độ di chuyển
    [SerializeField] private float jumpForce = 12f; // Lực nhảy
    [SerializeField] private float TimeOutWall;
    [SerializeField] private float dashDistance = 10f; // Khoảng cách lướt
    [SerializeField] private float dashDuration = 0.2f; // Thời gian lướt

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
    private bool isDashing; // Kiểm tra có đang lướt không

    // Attack-related variables
    private AudioSource audioSource;
    [SerializeField] private AudioClip jumpSound; // Âm thanh nhảy
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip runSound; // Âm thanh chạy
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private AudioClip dashSound; // Âm thanh lướt


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
        if (Input.GetKeyDown(KeyCode.F) && !isDashing && !isTouchingWall) // Kiểm tra lướt
        {

            StartCoroutine(Dash());
            return;
        }
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
            }
        }

        // Xoay sprite theo hướng
        if (horizontal > 0) sprite.flipX = false;
        else if (horizontal < 0) sprite.flipX = true;

        // Gửi trạng thái vào Animator
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        if (!isDashing) // Kiểm tra nếu không lướt
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
    }
    void UpdateAnimations()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;

        if (isWallSliding)
        {
            animator.Play("WallSlide");
        }
        else if (isDashing)
        {
            animator.Play("Dash");
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
        bullet.GetComponent<Bullet_Cong>().SetDirection(direction);
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        float originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;

        PlaySound(dashSound);

        // Determine the dash direction
        Vector2 dashDirection = sprite.flipX ? Vector2.left : Vector2.right;

        // Apply the dash force
        rb.velocity = new Vector2(dashDirection.x * dashDistance, rb.velocity.y);

        // Wait for the duration of the dash
        yield return new WaitForSeconds(dashDuration);

        // Reset the Rigidbody's gravity scale and dashing state
        rb.gravityScale = originalGravityScale;
        isDashing = false;
    }
    private void PlayJumpSound()
    {
        PlaySound(jumpSound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    void OnCollisionEnter2D(Collision2D collison)
    {
        // Cập nhật trạng thái va chạm với groundLayer và wallLayer
        if (((1 << collison.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
            canJump = true;
        }

        if (((1 << collison.gameObject.layer) & wallLayer) != 0)
        {
            isTouchingWall = true;
            canJump = true;
        }
    }

    void OnCollisionStay2D(Collision2D collison)
    {
        if (((1 << collison.gameObject.layer) & groundLayer) != 0)
        {
            canJump = true;
        }

        if (((1 << collison.gameObject.layer) & wallLayer) != 0)
        {
            isTouchingWall = true;
            canJump = true;
        }
    }

    void OnCollisionExit2D(Collision2D collison)
    {
        if (((1 << collison.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = false;
            canJump = false;
        }

        if (((1 << collison.gameObject.layer) & wallLayer) != 0)
        {
            isTouchingWall = false;
            canJump = false;
        }
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        // Cập nhật trạng thái va chạm với groundLayer và wallLayer
        if (((1 << collider.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
            canJump = true;
        }

        if (((1 << collider.gameObject.layer) & wallLayer) != 0)
        {
            isTouchingWall = true;
            canJump = true;
        }
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        if (((1 << collider.gameObject.layer) & groundLayer) != 0)
        {
            canJump = true;
        }

        if (((1 << collider.gameObject.layer) & wallLayer) != 0)
        {
            isTouchingWall = true;
            canJump = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (((1 << collider.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = false;
            canJump = false;
        }

        if (((1 << collider.gameObject.layer) & wallLayer) != 0)
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