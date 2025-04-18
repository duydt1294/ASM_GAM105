using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
	public GameObject bulletPrefab;        // Prefab viên đạn
	public Transform firePoint;            // Vị trí sinh đạn
	public float bulletSpeed = 10f;        // Tốc độ bay của đạn

	private SpriteRenderer spriteRenderer;

	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0)) // Nhấn chuột trái
		{
			Shoot();
		}
	}

	void Shoot()
	{
		// Tạo đạn tại vị trí firePoint
		GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

		// Lấy hướng nhân vật đang quay mặt
		int direction = spriteRenderer.flipX ? -1 : 1;

		// Đẩy đạn về phía trước mặt nhân vật
		Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
		if (rb != null)
		{
			rb.velocity = new Vector2(direction * bulletSpeed, 0);
		}

		// Lật đạn theo hướng bắn (nếu bạn có animation flip)
		SpriteRenderer bulletSprite = bullet.GetComponent<SpriteRenderer>();
		if (bulletSprite != null)
		{
			bulletSprite.flipX = spriteRenderer.flipX;
		}
	}
}