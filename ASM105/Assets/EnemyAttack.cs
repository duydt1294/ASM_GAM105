using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    private float lastAttackTime = 0f;

    public int damage = 10;

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                Attack(player);
                lastAttackTime = Time.time;
            }
        }
    }

    void Attack(GameObject player)
    {
        Debug.Log("Enemy Attack Player!");
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }
}