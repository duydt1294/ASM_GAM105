using UnityEngine;
using System.Collections;

public class KeThuAttack : MonoBehaviour
{
    private Animator animator;
    private bool isAttacking = false; // Biến kiểm soát trạng thái tấn công

    public float attackRange = 4f; // Phạm vi tấn công
    public float attackCooldown = 1f; // Thời gian cooldown giữa các lần tấn công
    private float lastAttackTime = 0f; // Thời điểm tấn công cuối cùng

    private Transform player; // Lưu vị trí người chơi

    public int damage = 10; // Sát thương tấn công

    void Start()
    {
        animator = GetComponent<Animator>(); // Lấy Animator từ đối tượng
        player = GameObject.FindGameObjectWithTag("Player").transform; // Lấy Transform của người chơi
    }

    void Update()
    {
        if (player != null)
        {
            // Tính khoảng cách giữa kẻ thù và người chơi
            float distance = Vector3.Distance(transform.position, player.position);

            // Kiểm tra nếu kẻ thù ở trong phạm vi tấn công và có đủ thời gian cooldown
            if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown && !isAttacking)
            {
                isAttacking = true;
                animator.SetBool("isAttacking", true); // Bật trạng thái tấn công
                lastAttackTime = Time.time;
                StartCoroutine(AttackDelay());
            }
        }
    }

    // Coroutine để reset trạng thái tấn công sau khi animation kết thúc
    private IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(0.8f); // Thời gian animation tấn công
        OnAttack(); // Gọi hàm tấn công tại thời điểm đánh trúng
        isAttacking = false;
        animator.SetBool("isAttacking", false); // Tắt trạng thái tấn công
    }

    // Hàm tấn công khi animation tấn công kết thúc
    void OnAttack()
    {
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            PlayerHealth1 playerHealth = player.GetComponent<PlayerHealth1>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Kẻ thù tấn công người chơi! Gây " + damage + " sát thương.");
            }
            else
            {
                Debug.LogWarning("Không tìm thấy component PlayerHealth1 trên người chơi!");
            }
        }
    }


    // Hiển thị phạm vi tấn công trong Scene view (chỉ để debug)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); // Vẽ phạm vi tấn công
    }
}

