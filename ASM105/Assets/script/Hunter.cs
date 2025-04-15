using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    public float moveDistance = 2f;      // khoảng cách tối đa di chuyển từ vị trí ban đầu
    public float moveSpeed = 1f;         // tốc độ di chuyển tuần tra
    private Vector3 startPosition;       // lưu vị trí ban đầu của Hunter
    private bool movingRight = true;     // cờ để biết đang đi sang phải hay trái

    public Transform Player;             // Player được gán từ Inspector
    public float detectionRange = 5f;    // phạm vi phát hiện Player
    public float attackSpeed = 2f;       // tốc độ đuổi theo Player
    public float stopDistance = 0.5f;    // khoảng cách để dừng lại khi đã bắt được Player

    //private bool isAttacking = false;    // cờ kiểm tra đang tấn công hay không

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        startPosition = transform.position; // lưu lại vị trí ban đầu
        spriteRenderer = GetComponent<SpriteRenderer>(); // lấy SpriteRenderer để lật mặt
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, Player.position);

        if (distance < detectionRange)
        {
            // Phát hiện Player
          //  isAttacking = true;
            AttackPlayer();
        }
        else
        {
            // Không thấy Player → tuần tra bình thường
          //  isAttacking = false;
            Patrol();
        }
    }

    // Di chuyển tuần tra trái - phải khi không tấn công
    void Patrol()
    {
        if (movingRight)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;

            if (transform.position.x >= startPosition.x + moveDistance)
                movingRight = false;

            spriteRenderer.flipX = false; // nhìn sang phải
        }
        else
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;

            if (transform.position.x <= startPosition.x - moveDistance)
                movingRight = true;

            spriteRenderer.flipX = true; // nhìn sang trái
        }
    }

    // Di chuyển và lật mặt khi đang đuổi theo Player
    void AttackPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, Player.position);

        if (distanceToPlayer > stopDistance)
        {
            // Nếu còn cách xa Player thì tiếp tục đuổi theo
            transform.position = Vector2.MoveTowards(transform.position, Player.position, attackSpeed * Time.deltaTime);

            // Lật mặt theo hướng Player
            FlipTowardsPlayer();
        }
        else
        {
            // Nếu đã tới gần Player → dừng lại, không xoay mặt nữa
            // Có thể thêm animation tấn công ở đây
        }
    }

    // Lật mặt Hunter theo vị trí Player
    void FlipTowardsPlayer()
    {
        spriteRenderer.flipX = Player.position.x < transform.position.x;
    }

    private void OnDrawGizmosSelected()
    {
        // Vẽ vùng phát hiện Player trong Scene View
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
