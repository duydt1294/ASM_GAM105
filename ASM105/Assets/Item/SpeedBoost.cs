using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
	public float speedMultiplier = 1.5f; // Tăng 50%
	public float boostDuration = 30f;     // Thời gian hiệu lực

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			PlayerController player = collision.GetComponent<PlayerController>();

			if (player != null)
			{
				player.ActivateSpeedBoost(speedMultiplier, boostDuration);
				Destroy(gameObject); // Biến mất sau khi nhặt
			}
		}
	}
}


//bổ sung code NV
/*
public class PlayerMovement : MonoBehaviour
{
	public float speed = 5f;
	private float originalSpeed;
	private bool isSpeedBoosted = false;

	void Start()
	{
		originalSpeed = speed;
	}

	void Update()
	{
		float moveX = Input.GetAxisRaw("Horizontal");
		float moveY = Input.GetAxisRaw("Vertical");

		transform.Translate(new Vector3(moveX, moveY, 0).normalized * speed * Time.deltaTime);
	}

	public void ActivateSpeedBoost(float multiplier, float duration)
	{
		if (!isSpeedBoosted)
		{
			StartCoroutine(SpeedBoost(multiplier, duration));
		}
	}

	private System.Collections.IEnumerator SpeedBoost(float multiplier, float duration)
	{
		isSpeedBoosted = true;
		speed *= multiplier;
		Debug.Log("Tăng tốc độ: " + speed);
		yield return new WaitForSeconds(duration);
		speed = originalSpeed;
		isSpeedBoosted = false;
		Debug.Log("Tốc độ trở lại bình thường: " + speed);
	}
}
*/