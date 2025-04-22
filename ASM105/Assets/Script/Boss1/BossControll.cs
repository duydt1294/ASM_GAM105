using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    //Combat
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float shootInterval = 2f;
    public float empowerDuration = 7f;
    public float empowerCooldown = 20f;

    //Di chuyển
    public float patrolSpeed = 2f;  // tốc độ boss di chuyển khi đi tuần tra
    public float chaseSpeed = 3f;   // tốc độ boss di chuyển khi đuổi theo player
    public float detectionRange = 5f; // khoảng cách phát hiện player
    public float leftLimit = -8f;
    public float rightLimit = 8f;

    //Máu
    public int maxHealth = 100;
    public Slider hpSlider;

    private int currentHealth;
    private bool isMoving = true;
    private bool isEmpowered = false;
    private bool isDead = false;
    private bool isCastingEmpower = false;

    private Rigidbody2D rb;
    private Animator animator;
    private Transform player;
    private bool movingRight = true;
    private float baseSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        currentHealth = maxHealth;
        //hpSlider.value = currentHealth;
        baseSpeed = patrolSpeed;

        InvokeRepeating(nameof(Shoot), 0f, shootInterval);
        StartCoroutine(EmpowerRoutine());
    }

    private void Update()
    {
        if (!isMoving || isDead || isCastingEmpower)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("isMoving", false);
            return;
        }

        float distToPlayer = Vector2.Distance(transform.position, player.position);
        if (distToPlayer < detectionRange)
        {
            animator.SetBool("isMoving", true);
            ChasePlayer();
        }
        else
        {
            animator.SetBool("isMoving", true);
            Patrol();
        }
    }

    // ===================== CHIÊU 1 =====================
    void Shoot()
    {
        if (isDead || isCastingEmpower) return;
        StartCoroutine(ShootRoutine());
    }

    IEnumerator ShootRoutine()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f); // thời gian animation

        Quaternion rot = transform.rotation * Quaternion.Euler(0, 180f, 0);
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, rot);
        GhostProjectile ghost = projectile.GetComponent<GhostProjectile>();
        if (ghost != null)
        {
            ghost.SetDirection(transform.localScale.x);
            ghost.SetEmpowered(isEmpowered);
        }

        yield return null;
    }

    // ===================== CHIÊU 2 =====================
    IEnumerator EmpowerRoutine()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(empowerCooldown);

            isCastingEmpower = true;
            isMoving = false;
            rb.velocity = Vector2.zero;

            animator.SetTrigger("Attack2"); // animation gồng skill
            yield return new WaitForSeconds(1.2f); // thời gian animation
            yield return new WaitForSeconds(1f); 

            isEmpowered = true;
            isCastingEmpower = false;
            isMoving = true;

            yield return new WaitForSeconds(empowerDuration);
            isEmpowered = false;
        }
    }

    // ===================== DI CHUYỂN =====================
    void Patrol()
    {
        if (movingRight)
        {
            rb.velocity = Vector2.right * baseSpeed;
            transform.rotation = Quaternion.Euler(0, 180, 0);
            if (transform.position.x >= rightLimit)
                movingRight = false;
        }
        else
        {
            rb.velocity = Vector2.left * baseSpeed;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            if (transform.position.x <= leftLimit)
                movingRight = true;
        }
    }

    void ChasePlayer()
    {
        Vector2 dir;
        if (player.position.x > transform.position.x)
        {
            dir = Vector2.right;
            if (!movingRight)
            {
                movingRight = true;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else
        {
            dir = Vector2.left;
            if (movingRight)
            {
                movingRight = false;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        rb.velocity = dir * chaseSpeed;
    }

    // ===================== VA CHẠM & MÁU =====================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("hitbox") && !isDead)
        {
            TakeDamage(10);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        hpSlider.value = currentHealth;

        StartCoroutine(HurtAnim());

        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator HurtAnim()
    {
        animator.SetBool("TakeDamage", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("TakeDamage", false);
    }

    IEnumerator Die()
    {
        isDead = true;
        isMoving = false;
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Die");

        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
