using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Di chuyển")]
    [SerializeField] private float moveSpeed = 5f; // Tốc độ di chuyển
    [SerializeField] private float jumpForce = 12f; // Lực nhảy
    [SerializeField] private float wallSlideSpeed = 1f; // Tốc độ trượt tường

    [Header("Kiểm tra va chạm")]
    [SerializeField] private LayerMask groundLayer; // Layer cho mặt đất
    [SerializeField] private LayerMask wallLayer; // Layer cho tường

    private Rigidbody2D rb; // Rigidbody2D của nhân vật
    private Animator animator; // Animator để điều khiển animation
    private SpriteRenderer sprite; // SpriteRenderer để thay đổi hình ảnh

    private float horizontal; // Giá trị di chuyển ngang
    private bool isGrounded; // Kiểm tra có đứng trên mặt đất không
    private bool isWallSliding; // Kiểm tra có đang bám tường không
    private bool isTouchingWall; // Kiểm tra có chạm vào tường không
    private bool canJump; // Kiểm tra có thể nhảy không
    private int wallDirection; // Hướng bám tường

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Lấy Rigidbody2D
        animator = GetComponent<Animator>(); // Lấy Animator
        sprite = GetComponent<SpriteRenderer>(); // Lấy SpriteRenderer
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); // Lấy giá trị di chuyển ngang

        // Kiểm tra bám tường
        if (isTouchingWall && !isGrounded && Mathf.Abs(horizontal) > 0)
        {
            isWallSliding = true; // Bắt đầu bám tường
        }
        else
        {
            isWallSliding = false; // Ngừng bám tường
        }

        // Nhảy khi chạm bất kỳ collider nào
        if (Input.GetButtonDown("Jump") && canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Nhảy lên
            isWallSliding = false; // Ngừng bám tường khi nhảy
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

        // Bám tường, giảm tốc độ di chuyển khi bám tường
        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed); // Giảm tốc độ khi bám tường
        }
    }

    void UpdateAnimations()
    {
        if (isWallSliding)
        {
            animator.Play("WallSlide"); // Gọi animation bám tường
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

    // Kiểm tra va chạm với các collider
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Chạm vào bất kỳ collider nào cho phép nhảy
        canJump = true;

        // Nếu là tường thì bật cờ trượt tường
        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            isTouchingWall = true; // Đánh dấu chạm tường
            wallDirection = collision.contacts[0].normal.x > 0 ? 1 : -1; // Xác định hướng bám tường
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
}