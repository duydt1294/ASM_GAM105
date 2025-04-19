using UnityEngine;

public class Quai3 : MonoBehaviour
{
    public float speed = 2f;
    public float patrolRange = 2f;
    public float chaseRange = 3f;
    public Transform player;

    private Vector3 startPos;
    private bool goingForward = true;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isTouchingPlayer = false;
    private bool isAttacking = false;

    void Start()
    {
        startPos = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    void Update()
    {
        if (isAttacking) return; // ❌ Không di chuyển khi đang attack

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            Vector3 targetPos = player.position;
            targetPos.x = Mathf.Clamp(targetPos.x, startPos.x, startPos.x + patrolRange);
            targetPos.y = transform.position.y;

            Vector3 direction = (targetPos - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            if (Mathf.Abs(targetPos.x - transform.position.x) > 0.1f)
            {
                spriteRenderer.flipX = (targetPos.x < transform.position.x);
            }
        }
        else
        {
            if (goingForward)
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
                spriteRenderer.flipX = false;

                if (transform.position.x >= startPos.x + patrolRange)
                {
                    goingForward = false;
                }
            }
            else
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
                spriteRenderer.flipX = true;

                if (transform.position.x <= startPos.x)
                {
                    goingForward = true;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("player"))
        {
            isTouchingPlayer = true;
            isAttacking = true;

            if (animator != null)
            {
                animator.SetBool("IsAttacking", true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("player"))
        {
            isTouchingPlayer = false;
            isAttacking = false;

            if (animator != null)
            {
                animator.SetBool("IsAttacking", false);
            }
        }
    }
}
