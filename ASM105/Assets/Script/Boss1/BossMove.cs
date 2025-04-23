using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class boss : MonoBehaviour
{
    private float speed = 2;
    private float speedGoc;
    private Rigidbody2D rb;
    private Animator animator;
    public float PhamViPhatHienNhanVat = 5f;
    public float speedRuotDuoi = 3f;
    public float lw = -8f;
    public float rw = 8f;
    private Transform player;
    private bool movingRight = true;

    int mautoida = 100;
    int mauhientai;
    [SerializeField] Slider hp;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        speedGoc = speed;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        mauhientai = mautoida;
        hp.value = mauhientai;
    }

    void Update()
    {
            
        if (!isWalking)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        float KhoangCachToiNhanVat = Vector2.Distance(transform.position, player.position);
        if (KhoangCachToiNhanVat < PhamViPhatHienNhanVat)
        {
            DuoiTheoNhanVat();
        }
        else
        {
            TuanTra();
        }
        //animator.Play("Move");
    }
    void TuanTra()
    {
        if (movingRight)
        {
            speed = speedGoc;
            rb.velocity = Vector2.right * speed;
            transform.rotation = Quaternion.Euler(0, 180, 0);
            if (transform.position.x >= rw)
            {
                movingRight = false;
            }
        }
        else
        {
            rb.velocity = Vector2.left * speed;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            if (transform.position.x <= lw)
            {
                movingRight = true;
            }
        }
    }
    private bool isWalking = true;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.gameObject.name == "man" && isWalking)
        {
            StartCoroutine(WaitBeforeMove(2f));
        }
    }

    IEnumerator WaitBeforeMove(float time)
    {
        isWalking = false;
        float oldSpeed = speed;
        speed = 0;
        rb.velocity = Vector2.zero; // Dừng di chuyển hẳn
        yield return new WaitForSeconds(time);
        speed = oldSpeed;
        isWalking = true;
        Debug.Log("Boss đang dừng lại trong 2 giây");
    }
    void DuoiTheoNhanVat()
    {
        speed = speedRuotDuoi; // Tăng tốc độ khi rượt đuổi
        Vector2 PhuongHuong;
        if (player.position.x > transform.position.x)
        // Nếu player.x lớn hơn quái.x → tức là nhân vật đang ở bên phải quái.
        {
            PhuongHuong = Vector2.right;
            if (!movingRight) // Nếu quái đang không nhìn sang phải (đang nhìn trái)
            {
                movingRight = true;
                //rb.velocity = Vector2.right * speed;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else //Nếu player.x < quái.x → quái phải rượt sang trái.
        {
            PhuongHuong = Vector2.left;
            // Vì nhân vật đang ở bên trái (x nhỏ hơn), nên quái phải đi sang trái
            if (movingRight)  // Nếu quái đang nhìn phải, mà giờ phải quay trái
            {
                movingRight = false; // Quay lại hướng trái
                //rb.velocity = Vector2.left * speed;
                transform.rotation = Quaternion.Euler(0, 0, 0);// Lật sprite để quay mặt qua trái
            }
        }
        rb.velocity = PhuongHuong * speed;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PhamViPhatHienNhanVat);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("hitbox"))
        {
            mauhientai -= 10;
            hp.value = mauhientai;
            StartCoroutine(chayAnimation());
        }
        if (mauhientai <= 0)
        {
            animator.SetTrigger("Die");
            rb.bodyType = RigidbodyType2D.Static; // Dừng lại
            Destroy(gameObject, 1.5f);
        }
    }
    IEnumerator chayAnimation()
    {
        animator.SetBool("takedamage", true); // Dùng bool để kích hoạt animation
        yield return new WaitForSeconds(0.7f);
        animator.SetBool("takedamage", false); // Tắt animation
    }
}