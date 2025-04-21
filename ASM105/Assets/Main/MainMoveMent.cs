
using UnityEngine;

public class MainMoveMent : MonoBehaviour
{
    public float speed = 5f;
    public float JumpForce = 7f;
    private bool isGrounded = true;
    private Rigidbody2D rb;
    private Vector2 moveMent;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        moveMent.x = Input.GetAxis("Horizontal");
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetInteger("TrangThai", 1);
        }
        else
        {
            animator.SetInteger("TrangThai", 0);
        }

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            spriteRenderer.flipX = true;
        }else if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            spriteRenderer.flipX = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("Đã nhấn nhảy!");
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
            isGrounded = false;
            
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveMent.x * speed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Va chạm với: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

}
