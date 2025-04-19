using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
	[Header("Máu")]
	public int maxHealth = 100;
	public int currentHealth;

	[Header("Tấn công")]
	public float baseAttack = 10f;
	public float currentAttack;

	[Header("Tốc độ")]
	public float moveSpeed = 5f;
	private float originalSpeed;

	[Header("Âm thanh")]
	public AudioClip takeItemSound;
	private AudioSource audioSource;

	private bool isSpeedBoosted = false;
	private bool isAttackBoosted = false;
	private bool isInWater = false;

	private Rigidbody2D rb;
	private Vector2 movement;

	private Coroutine waterDamageCoroutine;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		audioSource = GetComponent<AudioSource>();
		currentHealth = maxHealth;
		currentAttack = baseAttack;
		originalSpeed = moveSpeed;
	}

	void Update()
	{
		movement.x = Input.GetAxisRaw("Horizontal");
		movement.y = Input.GetAxisRaw("Vertical");
	}

	void FixedUpdate()
	{
		rb.velocity = movement.normalized * moveSpeed;
	}

	public void Heal(int amount)
	{
		currentHealth += amount;
		currentHealth = Mathf.Min(currentHealth, maxHealth);
		Debug.Log("🩹 Hồi máu: " + amount + " → Máu hiện tại: " + currentHealth);
		PlayTakeItemSound();
	}

	public void ActivateSpeedBoost(float multiplier, float duration)
	{
		if (!isSpeedBoosted)
		{
			StartCoroutine(SpeedBoost(multiplier, duration));
		}
	}

	private IEnumerator SpeedBoost(float multiplier, float duration)
	{
		isSpeedBoosted = true;
		moveSpeed *= multiplier;
		PlayTakeItemSound();
		Debug.Log("⚡ Tăng tốc độ: " + moveSpeed);
		yield return new WaitForSeconds(duration);
		moveSpeed = originalSpeed;
		isSpeedBoosted = false;
		Debug.Log("🏃‍♂️ Tốc độ trở lại bình thường: " + moveSpeed);
	}

	public void ActivateAttackBoost(float multiplier, float duration)
	{
		if (!isAttackBoosted)
		{
			StartCoroutine(AttackBoost(multiplier, duration));
		}
	}

	private IEnumerator AttackBoost(float multiplier, float duration)
	{
		isAttackBoosted = true;
		currentAttack *= multiplier;
		PlayTakeItemSound();
		Debug.Log("🔥 Tăng sức mạnh: " + currentAttack);
		yield return new WaitForSeconds(duration);
		currentAttack = baseAttack;
		isAttackBoosted = false;
		Debug.Log("💪 Sức mạnh trở lại bình thường: " + currentAttack);
	}

	public float GetCurrentAttack()
	{
		return currentAttack;
	}

	private void PlayTakeItemSound()
	{
		if (takeItemSound != null && audioSource != null)
		{
			audioSource.PlayOneShot(takeItemSound);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Gai"))
		{
			TakeDamage(10);
			Debug.Log("🩸 Đụng gai! Máu hiện tại: " + currentHealth);
		}

		if (collision.CompareTag("Water"))
		{
			isInWater = true;
			if (waterDamageCoroutine == null)
			{
				waterDamageCoroutine = StartCoroutine(WaterDamageOverTime());
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Water"))
		{
			isInWater = false;
			if (waterDamageCoroutine != null)
			{
				StopCoroutine(waterDamageCoroutine);
				waterDamageCoroutine = null;
				Debug.Log("🚪 Rời khỏi nước!");
			}
		}
	}

	private IEnumerator WaterDamageOverTime()
	{
		Debug.Log("🌊 Ở trong nước → bắt đầu mất máu mỗi giây");
		while (isInWater)
		{
			TakeDamage(5);
			Debug.Log("🌊 Mất máu do nước → Máu hiện tại: " + currentHealth);
			yield return new WaitForSeconds(1f);
		}
	}

	public void TakeDamage(int damage)
	{
		currentHealth -= damage;
		currentHealth = Mathf.Max(0, currentHealth);

		if (currentHealth <= 0)
		{
			Debug.Log("💀 Nhân vật đã chết!");
			// Thêm hiệu ứng chết, game over tại đây nếu cần
		}
	}
}
