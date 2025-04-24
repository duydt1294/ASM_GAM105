using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Quai3 : MonoBehaviour
{
    public int mautoida = 100;
    public int mauhientai;
    Animator quai3, quai3chet;

    //hau
    //public float speed = 2f;
    //public float patrolRange = 2f;
    //public float chaseRange = 3f;
    //public Transform player;
    //private Vector3 startPos;
    //private bool goingForward = true;
    //private SpriteRenderer spriteRenderer;
    private Animator animator, run, tancong;
    //private bool isTouchingPlayer = false;
    //private bool isAttacking = false;
    void Start()
    {
        mauhientai = mautoida;
        quai3 = GetComponent<Animator>();
        quai3chet = GetComponent<Animator>();
        //hau
        //startPos = transform.position;
        //spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        run = GetComponent<Animator>();
        tancong = GetComponent<Animator>();

        //if (player == null)
        //{
        //    GameObject playerObj = GameObject.FindGameObjectWithTag("player");
        //    if (playerObj != null)
        //    {
        //        player = playerObj.transform;
        //    }
        //}

    }
    void Update()
    {
        //hau
        //if (isAttacking) return; // ❌ Không di chuyển khi đang attack

        //float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        //if (distanceToPlayer <= chaseRange)
        //{
        //    Vector3 targetPos = player.position;
        //    targetPos.x = Mathf.Clamp(targetPos.x, startPos.x, startPos.x + patrolRange);
        //    targetPos.y = transform.position.y;

        //    Vector3 direction = (targetPos - transform.position).normalized;
        //    transform.position += direction * speed * Time.deltaTime;
        //    // y != 0
        //    animator.SetBool("run", true);

        //    if (Mathf.Abs(targetPos.x - transform.position.x) > 0.1f)
        //    {
        //        spriteRenderer.flipX = !(targetPos.x < transform.position.x);
        //    }
        //}
        //else
        //{
        //    animator.SetBool("run", true);

        //    if (goingForward)
        //    {
        //        transform.position += Vector3.right * speed * Time.deltaTime;
        //        spriteRenderer.flipX = true;

        //        if (transform.position.x >= startPos.x + patrolRange)
        //        {
        //            goingForward = false;
        //        }
        //    }
        //    else
        //    {
        //        transform.position += Vector3.left * speed * Time.deltaTime;
        //        spriteRenderer.flipX = false;

        //        if (transform.position.x <= startPos.x)
        //        {
        //            goingForward = true;
        //        }
        //    }
        //}
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            mauhientai -= 35;
            StartCoroutine(chayAnimation()); // Gọi hàm IEnumerator bên dưới để Bắt đầu animation
        }
        if (mauhientai <= 0)
        {
            quai3chet.SetTrigger("chet");
            Destroy(gameObject, 1f);
        }
    }
    //public void OnCollisionEnter2D(Collision2D collision) // Bắt sự kiện khi va chạm với Player thì sẽ đấm liên tục
    //{
    //    tancong.SetBool("Attack", true);
    //}
    //public void OnCollisionExit2D(Collision2D collision) // bắt sự kiện khi thoát va chạm thì sẽ ngừng đấm
    //{
    //    tancong.SetBool("Attack", false);
    //}

    IEnumerator chayAnimation()
        {
            quai3.SetBool("NhanSt", true); // Dùng bool để kích hoạt animation
            yield return new WaitForSeconds(0.3f); // Đợi animation chạy trong 0.3s
            quai3.SetBool("NhanSt", false); // Tắt animation
        

        }
}
    

