using System.Collections;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float shootInterval = 2f;

    private Animator animator;

    private bool isEmpowered = false;
    private float empowerDuration = 7f;
    private float empowerCooldown = 20f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        InvokeRepeating(nameof(Shoot), 0f, shootInterval);
        StartCoroutine(EmpowerRoutine()); // Bắt đầu đếm giờ chiêu 2
    }

    void Shoot()
    {
        StartCoroutine(ShootRoutine());
    }

    IEnumerator ShootRoutine()
    {
        //animator.SetBool("isShooting", true);
        animator.SetTrigger("iisShooting"); // Sử dụng trigger để kích hoạt animation bắn
        Quaternion rotation = transform.rotation;
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, rotation);

        // Gửi trạng thái cường hóa cho đạn
        GhostProjectile ghost = projectile.GetComponent<GhostProjectile>();
        if (ghost != null)
        {
            ghost.SetDirection(transform.localScale.x);
            ghost.SetEmpowered(isEmpowered); // Gửi flag cường hóa
        }
        yield return null;
        //yield return new WaitForSeconds(0.3f);
        //animator.SetBool("isShooting", false);
    }

    IEnumerator EmpowerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(empowerCooldown);

            // Gồng animation chiêu 2
            animator.SetTrigger("Attack2"); // Sử dụng trigger để kích hoạt animation gồng
            yield return null; // thời gian animation gồng
            

            isEmpowered = true;
            Debug.Log("Boss đã cường hóa!");

            yield return new WaitForSeconds(empowerDuration);

            isEmpowered = false;
            Debug.Log("Boss hết cường hóa.");
        }
    }
}
