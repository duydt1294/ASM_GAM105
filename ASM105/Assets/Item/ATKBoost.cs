using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATKBoost : MonoBehaviour
{
	public float damageMultiplier = 1.5f; // Tăng 50%
	public float boostDuration = 30f; //thời gian hiệu lực (giây)

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ActivateAttackBoost(damageMultiplier, boostDuration);
                Destroy(gameObject); // Biến mất sau khi nhặt
            }
        }
	}
}



