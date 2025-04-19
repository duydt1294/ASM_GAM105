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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Lấy Rigidbody2D
        animator = GetComponent<Animator>(); // Lấy Animator
        sprite = GetComponent<SpriteRenderer>(); // Lấy SpriteRenderer
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); // Lấy giá trị di chuyển ngang

        // Kiểm tra bám tường chỉ khi nhân vật chạm vào tường và không đứng trên mặt đất
        if (isTouchingWall && !isGrounded && rb.velocity.y <= 0)
        {
            isWallSliding = true; // Bắt đầu bám tường ngay lập tức khi chạm vào tường
        }
        else
        {
            isWallSliding = false; // Ngừng bám tường khi không còn chạm tường
        }

        // Kiểm tra nhảy ra khỏi tường khi nhấn nút nhảy
        if (Input.GetButtonDown("Jump") && canJump)
        {
            if (isWallSliding) // Nếu đang bám tường
            {
                isOutWallSliding = true; // Đánh dấu là nhảy ra khỏi tường
                rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Nhảy lên
                isWallSliding = false; // Ngừng bám tường khi nhảy
                StartCoroutine(TransitionToOutWallSliding()); // Gọi Coroutine để chuyển animation nhảy ra khỏi tường
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Nhảy lên bình thường
                animator.Play("Jump"); // Gọi animation nhảy ngay lập tức
            }
        }

        // Xoay sprite theo hướng
        if (horizontal > 0) sprite.flipX = false; // Hướng sang phải
        else if (horizontal < 0) sprite.flipX = true; // Hướng sang trái

        // Gửi trạng thái vào Animator
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        // Di chuyển ngang
        if (!isWallSliding) // Không cho di chuyển ngang khi đang bám tường
        {
            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        }

        // Bám tường, chỉ cho phép di chuyển ngang và không vặn bị rơi
        if (isWallSliding)
        {
            rb.velocity = new Vector2(horizontal * moveSpeed, 0); // Giữ vận tốc y bằng 0 khi bám tường
            rb.gravityScale = 0; // Tắt trọng lực khi bám tường
        }
        else
        {
            rb.gravityScale = 1; // Khôi phục trọng lực khi không bám tường
        }
    }

    void UpdateAnimations()
    {
        // Chuyển animation tuỳ theo trạng thái
        if (isWallSliding && !isOutWallSliding)
        {
            animator.Play("WallSlide"); // Gọi animation bám tường
        }
        else if (isOutWallSliding)
        {
            animator.Play("OutWallSlide"); // Gọi animation nhảy ra khỏi tường
        }
        else if (rb.velocity.y != 0) // Kiểm tra nếu đang nhảy
        {
            animator.Play("Jump"); // Gọi animation nhảy
        }
        else if (Mathf.Abs(horizontal) == 0)
        {
            animator.Play("Idle"); // Gọi animation đứng im
        }
        else
        {
            animator.Play("Run"); // Gọi animation chạy
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Chạm vào bất kỳ collider nào cho phép nhảy
        canJump = true;

        // Nếu là tường thì bật cờ trượt tường
        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            isTouchingWall = true; // Đánh dấu chạm tường
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Chạm vào bất kỳ collider nào cho phép nhảy
        canJump = true;

        // Nếu là tường thì vẫn đang chạm
        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            isTouchingWall = true; // Vẫn đang chạm tường
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Khi rời collider
        canJump = false; // Không thể nhảy khi không còn chạm vào collider

        // Nếu là tường thì không còn chạm vào tường
        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            isTouchingWall = false; // Không còn chạm vào tường
        }
    }

    // Coroutine để đảm bảo chuyển animation qua Jump sau khi ra khỏi tường
    private IEnumerator TransitionToOutWallSliding()
    {
        // Chờ một thời gian ngắn để animation OutWallSlide hiển thị
        yield return new WaitForSeconds(0.1f);

        // Gọi animation nhảy ra khỏi tường
        animator.Play("OutWallSlide");

        // Sau khi animation OutWallSlide kết thúc, chuyển ngay sang Jump
        yield return new WaitForSeconds(TimeOutWall); // Đợi một khoảng thời gian cho animation OutWallSlide

        // Sau khi hết thời gian của OutWallSlide, chuyển sang Jump
        animator.Play("Jump"); // Gọi animation nhảy

        // Reset trạng thái nhảy ra khỏi tường
        isOutWallSliding = false;
    }
}
