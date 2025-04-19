using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    Animator animator;
    bool chuadie = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!chuadie && other.CompareTag("bullet"))
        {
            chuadie = true;
            animator.SetBool("boss1die", true);
            StartCoroutine(DestroyAfterAnimation());
        }
    }

    IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(1f); // hoặc đúng thời gian animation die
        Destroy(gameObject);
    }
}
