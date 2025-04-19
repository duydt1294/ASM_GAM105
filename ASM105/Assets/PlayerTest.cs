using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveDirection;

    [SerializeField] private Vector3 portalDestination = new Vector3(357.58f, -22.65f, 0f);

    private Vector3 spawnPoint;

    void Start()
    {
        spawnPoint = transform.position;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Portal"))
        {
            transform.position = portalDestination;
        }
        else if (collision.gameObject.CompareTag("Lava"))
        {
            transform.position = spawnPoint;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
            }
        }
    }
}
