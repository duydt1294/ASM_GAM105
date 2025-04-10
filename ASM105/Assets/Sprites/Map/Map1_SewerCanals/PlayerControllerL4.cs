using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Thêm thư viện để load scene

public class PlayerControllerL4 : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    Transform playerTransform;
	Rigidbody2D rb;
	[SerializeField] float speed = 3.5f;
    Vector3 startPosition; //vi tri ban dau
	//[SerializeField] LuuDuLieu duLieu;

	Vector2[] viTriSpawn = { new Vector2(-3.68f, -1.22f), new Vector2(3.94f, 1.33f) };
	int index = 0; // Chỉ mục bắt đầu từ 0
	void Start()
    {
        playerTransform = GetComponent<Transform>();
		rb = GetComponent<Rigidbody2D>(); //Thay thế di chuyển transform bằng Rigidbody2D
		startPosition = transform.position;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

		//Vector3 movement = new Vector3(horizontal, vertical, 0).normalized;
		//playerTransform.Translate(movement * speed * Time.deltaTime); //time.deltaTime là giá trị thời gian thực, tránh ảnh hưởng bởi FPS
		//time.deltaTime = 1/fps . fps = 60 => 1/60

		Vector2 movement = new Vector2(horizontal, vertical).normalized;
		rb.velocity = movement * speed ; // Dùng velocity thay vì transform.position

		//Fix trượt khi di chuyển
		if (Input.GetAxisRaw("Horizontal") == 0)
		{
			rb.velocity = new Vector2(0, rb.velocity.y); // Chỉ dừng trượt ngang
		}
		if (Input.GetAxisRaw("Vertical") == 0)
		{
			rb.velocity = new Vector2(rb.velocity.x, 0); // Chỉ dừng trượt dọc
		}
	}


	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Tuong")) // Nếu chạm tường
		{
			rb.velocity = Vector2.zero; // Dừng di chuyển
		}

		if (collision.gameObject.tag.Equals("enemy"))
        {
			playerTransform.position = startPosition;
		}
        if(collision.gameObject.name.Equals("Win"))
        {
            if (GameObject.FindGameObjectsWithTag("enemy").Length < 6)
            {
				//float x = Random.Range(-0.8f, 0.8f);
				//float y = Random.Range(-3.2f, 3.2f);
				//Instantiate(enemy, new Vector3(x, y, 0f), Quaternion.identity);
				
				// Lấy vị trí theo thứ tự lần lượt
				Vector2 viTri = viTriSpawn[index];

				// Spawn quái tại vị trí đó
				Instantiate(enemy, viTri, Quaternion.identity);

				// Tăng chỉ mục, nếu vượt quá mảng thì quay lại vị trí đầu tiên
				index = (index + 1) % viTriSpawn.Length;
				
				playerTransform.position = startPosition;
			}
			else
			{
				//end game
				SceneManager.LoadScene("EndGameLab3");
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collider2D) // Chỉ enemy dùng Trigger
	{
		if (collider2D.gameObject.tag.Equals("enemy"))
		{
			playerTransform.position = startPosition;
			//duLieu.soLanChet++;
		}
	}
}
