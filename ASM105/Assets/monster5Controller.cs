using System.Collections;
using UnityEngine;

public class monster5Controller : MonoBehaviour
{
    [HideInInspector] public bool isWaiting = false;
    [SerializeField] GameObject dmgArea;
    private Animator animator;
    private float currentSpeed;
    private Transform enemyTransform;
    [HideInInspector] public Transform playerTransform;
    private bool isChasing = false;
    private bool isAttacking = false;

    public float normalSpeed = 2f;
    public float chaseSpeed = 30f;
    public float xoayHuong = -1f;
    public float leftLimit = -5f;
    public float rightLimit = 5f;
    public int HPMod = 100;
    public float attackDistance = 1.5f;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentSpeed = normalSpeed;
        enemyTransform = transform;
    }

    void Update()
    {
        if (isWaiting || HPMod <= 0) return;

        if (isChasing && playerTransform != null)
        {
            ChasePlayer();
        }
        else
        {
            Vector2 movement = new Vector2(-xoayHuong, 0f);
            transform.Translate(movement * currentSpeed * Time.deltaTime);
            animator.Play("diChuyen");
        }

    }

    public void PhatHienPlayer()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        currentSpeed = chaseSpeed;
        StartCoroutine(phatHienPlayer());
    }

    public void TanCongPlayer()
    {
        if (!isAttacking)
        {
            StartCoroutine(chuanBiTanCong());
        }
    }

    public void kiemTraMatDat()
    {
        StartCoroutine(quayDauLaBo());
    }

    public void diChuyenQuai()
    {
        isWaiting = false;
    }

    IEnumerator chuanBiTanCong()
    {
        isAttacking = true;
        isWaiting = true;
        animator.Play("attack");

        yield return new WaitForSeconds(0.5f);

        float huong = transform.localScale.x > 0 ? 1 : -1;
        float huongY = transform.localScale.y > 0 ? 1 : -1;
        Vector3 offset = new Vector3(1f * huong, -1.7f * huongY, 0);
        GameObject vungSatThuong = Instantiate(dmgArea, transform.position + offset, Quaternion.identity);
        Destroy(vungSatThuong, 0.7f);

        yield return new WaitForSeconds(1.5f);
        isAttacking = false;
        isWaiting = false;
    }

    IEnumerator quayDauLaBo()
    {
        isWaiting = true;
        animator.Play("Idle");
        yield return new WaitForSeconds(2f);

        xoayHuong *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        isWaiting = false;
    }

    IEnumerator phatHienPlayer()
    {
        isWaiting = true;
        animator.Play("Idle");
        yield return new WaitForSeconds(1f);
        isWaiting = false;
        isChasing = true;
    }

    void ChasePlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        //Flip đúng hướng
        if ((direction.x > 0 && transform.localScale.x < 0) || (direction.x < 0 && transform.localScale.x > 0))
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        if (distanceToPlayer > attackDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, currentSpeed * Time.deltaTime);
            animator.Play("diChuyen");
        }
        else
        {
            TanCongPlayer();
        }
    }
    public void kiemTraSatThuong()
    {
        if (HPMod > 0) { StartCoroutine(BiTrungDan()); }
    }

    IEnumerator BiTrungDan()
    {
        isWaiting = true;
        animator.Play("dmg");
        HPMod -= 50;
        yield return new WaitForSeconds(0.2f);
        isWaiting = false;
        if (HPMod <= 0)
        {
            quaiHetMau();
        }
    }
    public void quaiHetMau()
    {
        StartCoroutine(Die());
    }
    
    IEnumerator Die()
    {
        isWaiting = true;
        animator.Play("Die");
        yield return new WaitForSeconds(2.4f);
        Destroy(gameObject);
    }


}
