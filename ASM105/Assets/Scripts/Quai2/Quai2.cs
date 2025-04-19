using UnityEngine;

public class Quai2 : MonoBehaviour
{
    public float speed = 2f;
    public float patrolRange = 7f;

    private Vector3 startPos;
    private bool goingForward = true;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        startPos = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (goingForward)
        {
            // Đi tới điểm cách startPos
            transform.position += Vector3.right * speed * Time.deltaTime;
            if (spriteRenderer != null) spriteRenderer.flipX = false;

            if (transform.position.x >= startPos.x + patrolRange)
            {
                goingForward = false;
            }
        }
        else
        {
            // Quay lại vị trí ban đầu
            transform.position += Vector3.left * speed * Time.deltaTime;
            if (spriteRenderer != null) spriteRenderer.flipX = true;

            if (transform.position.x <= startPos.x)
            {
                goingForward = true;
            }
        }
    }
}
