using UnityEngine;

public class MeteorFly : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    public float fallSpeed = 5f;  
    public float moveSpeed = 3f; 

    public GameObject explosionEffect; 

    void Start()
    {
        
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
       
        rb.velocity = new Vector2(moveSpeed, -fallSpeed); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
     
        if (other.CompareTag("Dat") || other.CompareTag("Player"))
        {
            
            animator.SetTrigger("MeoteorCoi");

            
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);  
            }
            Destroy(gameObject, 0.1f); 
        }
    }
}
