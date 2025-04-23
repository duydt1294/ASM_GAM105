using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanCong_Dang : MonoBehaviour
{
	public float fireDamage = 10f;
	public float burnDuration = 3f;
	private bool isBurning = false;
	private Animator animator;

	void Start()
	{
		animator = GetComponent<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			ApplyFireDamage(other);

			if (!isBurning)
			{
				isBurning = true;
				StartCoroutine(BurnPlayer(other));
			}

			PlayFireAnimation();
		}
	}

	private void ApplyFireDamage(Collider2D player)
	{
		//PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
		//if (playerHealth != null)
		//{
		//	playerHealth.TakeDamage(fireDamage);
		//}
	}

	private IEnumerator BurnPlayer(Collider2D player)
	{
		float burnTime = 0f;

		while (burnTime < burnDuration)
		{
			ApplyFireDamage(player);
			burnTime += 1f;
			yield return new WaitForSeconds(1f);
		}

		isBurning = false;
		StopFireAnimation();
	}

	private void PlayFireAnimation()
	{
		if (animator != null)
		{
			animator.SetBool("IsFiring", true);
		}
	}

	private void StopFireAnimation()
	{
		if (animator != null)
		{
			animator.SetBool("IsFiring", false);
		}
	}
}
