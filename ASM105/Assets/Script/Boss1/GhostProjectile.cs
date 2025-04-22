using UnityEngine;

public class GhostProjectile : MonoBehaviour
{
    public float baseSpeed = 5f;
    public int baseDamage = 20;
    public float lifetime = 4f;
    public float slowDuration = 2f;

    private float speed;
    private int damage;
    private float direction = 1f;
    private bool isEmpowered = false;

    public void SetDirection(float scaleX)
    {
        direction = Mathf.Sign(scaleX);
        if (direction < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    public void SetEmpowered(bool empowered)
    {
        isEmpowered = empowered;

        if (isEmpowered)
        {
            speed = baseSpeed * 1.5f;
            damage = baseDamage + 10;
        }
        else
        {
            speed = baseSpeed;
            damage = baseDamage;
        }
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player1 player = collision.GetComponent<Player1>();
            if (player != null)
            {
                player.TakeDamage(damage);
                player.ApplySlow(slowDuration);

                if (isEmpowered)
                {
                    player.ApplyPoison(10f, 0.01f); // mất 1% máu mỗi giây trong 10s
                }
            }
            Destroy(gameObject);
        }
    }
}
