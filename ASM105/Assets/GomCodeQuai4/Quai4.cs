using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quai4 : MonoBehaviour
{
    public int mautoida = 100;
    public int mauhientai;
    Animator quai4;

    // Đối tượng quái để tham chiếu vị trí
    public Transform quai;

    // Thêm offset để thanh máu nằm đúng trên đầu quái
    public Vector3 offset = new Vector3(0, 1.5f, 0); // Điều chỉnh offset cho hợp lý


    void Start()
    {
        mauhientai = mautoida;
        quai4 = GetComponent<Animator>(); // Chỉ cần 1 Animator để điều khiển cả animation sống và chết
    }

    void Update()
    {
        if (quai != null)
        {
            // Chuyển đổi vị trí quái từ world space sang screen space
            Vector3 worldPos = quai.position + offset;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        }
    }

    IEnumerator chayAnimation()
    {
        quai4.Play("chet");
        yield return new WaitForSeconds(5f); // Đợi 0.3s
        //quai4.SetBool("NhanSt", false); // Tắt animation
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            mauhientai -= 30;
            quai4.Play("nhanst");
            //StartCoroutine(chayAnimation());

            // Kiểm tra nếu quái chính chết, ẩn thanh máu
            if (mauhientai <= 0)
            {
                StartCoroutine(chayAnimation());
                // Sử dụng bool "isDead" để chuyển sang animation chết
                Destroy(gameObject, 1f); // Hủy quái sau khi chết (sau 1s để animation chết hoàn thành)
            }
        }
    }
}
