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

	private Rigidbody2D rb;
	private Vector2 movement;

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
		// Di chuyển cơ bản
		movement.x = Input.GetAxisRaw("Horizontal");
		movement.y = Input.GetAxisRaw("Vertical");
	}

	void FixedUpdate()
	{
		rb.velocity = movement.normalized * moveSpeed;
	}

	// Hồi máu
	public void Heal(int amount)
	{
		currentHealth += amount;
		currentHealth = Mathf.Min(currentHealth, maxHealth);
		Debug.Log("🩹 Hồi máu: " + amount + " → Máu hiện tại: " + currentHealth);
		PlayTakeItemSound();
	}

	// Tăng tốc độ
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

	// Tăng sát thương
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

	// Truy cập sức tấn công hiện tại từ nơi khác
	public float GetCurrentAttack()
	{
		return currentAttack;
	}

	// Phát âm thanh khi nhặt vật phẩm
	private void PlayTakeItemSound()
	{
		if (takeItemSound != null && audioSource != null)
		{
			audioSource.PlayOneShot(takeItemSound);
		}
	}
}
