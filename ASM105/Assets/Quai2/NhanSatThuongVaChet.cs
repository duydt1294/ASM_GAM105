using System.Collections;
using UnityEngine;

public class NhanSatThuongVaChet_Thuan : MonoBehaviour
{
	public float maxHealth = 50f;
	[SerializeField] private float currentHealth;
	private Animator animator;
	private bool isDead = false;

	public float hurtAnimDuration = 0.5f; // thời gian animation NhanSatThuong

	void Start()
	{
		currentHealth = maxHealth;
		animator = GetComponent<Animator>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Bullet"))
		{
			TakeDamage(10f);
			Destroy(other.gameObject);
		}
	}

	public void TakeDamage(float damage)
	{
		if (isDead) return;

		currentHealth -= damage;

		// Phát animation bị thương, reset từ đầu
		if (animator != null)
		{
			StopAllCoroutines(); // nếu bị đánh liên tục thì dừng phát lại anim cũ
			animator.Play("NhanSatThuong", -1, 0f);
			StartCoroutine(BackToMove());
		}

		if (currentHealth <= 0)
		{
			StartCoroutine(Die());
		}
	}

	IEnumerator BackToMove()
	{
		yield return new WaitForSeconds(hurtAnimDuration);

		if (!isDead && animator != null)
		{
			animator.Play("DiChuyen");
		}
	}

	IEnumerator Die()
	{
		isDead = true;

		// Tắt khả năng di chuyển và tấn công
		MonoBehaviour moveScript = GetComponent<DiChuyen_Hau>();
		if (moveScript != null) moveScript.enabled = false;

		MonoBehaviour attackScript = GetComponent<TanCong_Dang>();
		if (attackScript != null) attackScript.enabled = false;

		// Phát animation chết
		if (animator != null)
		{
			animator.Play("Chet");
		}

		// Chờ 3 giây rồi xoá object
		yield return new WaitForSeconds(3f);
		Destroy(gameObject);
	}
}
