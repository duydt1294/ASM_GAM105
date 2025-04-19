using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quai4 : MonoBehaviour
{
    public int mautoida = 100;
    public int mauhientai;
    [SerializeField] Slider hpQuai;  // Thanh máu của quái chính
    Animator quai4;

    // Đối tượng quái để tham chiếu vị trí
    public Transform quai;

    // Thêm offset để thanh máu nằm đúng trên đầu quái
    public Vector3 offset = new Vector3(0, 1.5f, 0); // Điều chỉnh offset cho hợp lý

    public Quai4_Hiep quai4Hiep;  // Tham chiếu tới script Quai4_Hiep

    void Start()
    {
        mauhientai = mautoida;
        hpQuai.value = mauhientai;
        quai4 = GetComponent<Animator>(); // Chỉ cần 1 Animator để điều khiển cả animation sống và chết
    }

    void Update()
    {
        if (quai != null)
        {
            // Chuyển đổi vị trí quái từ world space sang screen space
            Vector3 worldPos = quai.position + offset;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

            // Cập nhật vị trí thanh máu theo vị trí quái trên màn hình
            hpQuai.transform.position = screenPos;
        }
        // Đoạn mã này dùng để kiểm tra (test) animation "nhận sát thương" và "chết" 
        // khi nhấn phím T. Có thể thay đổi logic tại đây để xử lý va chạm với player hoặc đạn.
        // Sau này:
        // - Kiểm tra khi viên đạn trúng quái.
        // - Gọi GiamMau để trừ máu và phát animation.
        if (Input.GetKeyDown(KeyCode.T)) // Khi nhấn phím T
        {   // Mã chỉ để test, thay đổi nếu cần
            // Giảm máu của quái 4
            mauhientai -= 30;
            hpQuai.value = mauhientai;
            StartCoroutine(chayAnimation());

            // Tác động vào máu của Quai4_Hiep
            if (quai4Hiep != null)
            {
                quai4Hiep.GiamMau(30); // Giảm 30 máu cho Quai4_Hiep
            }

            // Kiểm tra nếu quái chính chết, ẩn thanh máu
            if (mauhientai <= 0)
            {
                hpQuai.gameObject.SetActive(false); // Ẩn thanh slider của quái chính
                quai4.SetBool("chet", true); // Sử dụng bool "isDead" để chuyển sang animation chết
                Destroy(gameObject, 1f); // Hủy quái sau khi chết (sau 1s để animation chết hoàn thành)
            }
        }
    }

    IEnumerator chayAnimation()
    {
        quai4.SetBool("NhanSt", true); // Kích hoạt animation "nhận sát thương"
        yield return new WaitForSeconds(0.3f); // Đợi 0.3s
        quai4.SetBool("NhanSt", false); // Tắt animation
    }

    // Hàm giảm máu cho quái từ bên ngoài (gọi từ Quai4_Hiep)
    public void GiamMau(int soMau)
    {
        mauhientai -= soMau;
        hpQuai.value = mauhientai;
        StartCoroutine(chayAnimation());

        // Kiểm tra nếu quái chính chết, ẩn thanh máu
        if (mauhientai <= 0)
        {
            Debug.Log("Máu của quái chính bằng hoặc nhỏ hơn 0. Kích hoạt chết.");
            hpQuai.gameObject.SetActive(false); // Ẩn thanh slider của quái chính
            Debug.Log("Chuyển sang trạng thái chết");
            quai4.SetBool("chet", true); // Sử dụng bool "isDead" để chuyển sang animation chết
            Destroy(gameObject, 1f); // Hủy quái sau khi chết (sau 1s để animation chết hoàn thành)
        }
    }
}
