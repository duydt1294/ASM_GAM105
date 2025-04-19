using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDuy : MonoBehaviour
{
    Animator animator;
    bool lat_mat;
    public float MoveSpeed = 5f;

    private float moveX;
    private Vector2 movement;
    public float JumpForce = 10f;
    public LayerMask groundLayer;
    public BoxCollider2D GroundCollider;
    public bool OnGround;

    Rigidbody2D rb;
    public Transform matdat;
    public GameObject bulletPrefab;
    public Transform firePoint;

    private int jumpCount = 0; // Số lần đã nhảy
    private int maxJumps = 2; // Tối đa 2 lần
    //âm thanh
    private AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip shootSound;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        OnGround = true;
        jumpCount = 0;
    }

    void Update()
    {
        float trucX = 0f;
        if (Input.GetKey(KeyCode.A)) trucX = -1;
        if (Input.GetKey(KeyCode.D)) trucX = 1;

        if ((trucX < 0 && !lat_mat) || (trucX > 0 && lat_mat))
        {
            flip();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            animator.SetBool("Attack", true);
            Shoot();
            if (shootSound != null)
                audioSource.PlayOneShot(shootSound);
        }
        else
        {
            animator.SetBool("Attack", false);
        }
        // --------- Xử lý Double Jump ---------
        moveX = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            jumpCount++;
            OnGround = false;

            if (jumpSound != null)
                audioSource.PlayOneShot(jumpSound);
        }

        animator.SetFloat("Speed", Mathf.Abs(moveX));
        animator.SetBool("Jump", !OnGround);
    }

    
    void flip()
    {
        lat_mat = !lat_mat;
        Vector3 nv = transform.localScale;
        nv.x *= -1;
        transform.localScale = nv;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (groundLayer == (1 << collision.gameObject.layer))
        {
            OnGround = true;
            jumpCount = 0; // Reset số lần nhảy khi chạm đất
            animator.SetBool("Jump", false);
        }

        if (collision.gameObject.tag.Equals("kethu"))
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (groundLayer == (1 << collision.gameObject.layer))
        {
            OnGround = false;
            animator.SetBool("Jump", true);
        }
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        float direction = lat_mat ? -1f : 1f;
        bullet.GetComponent<bullet>().SetDirection(direction);
    }
}
