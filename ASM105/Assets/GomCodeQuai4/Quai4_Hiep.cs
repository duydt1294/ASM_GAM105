using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quai4_Hiep : MonoBehaviour
{
    Animator nhanSt;
    [SerializeField] Slider hp;
    public int hpHienTai;
    public int hpToiDa = 100;

    public Quai4 quai4Script;  // Tham chiếu đến script Quai4

    void Start()
    {
        hpHienTai = hpToiDa;
        hp.value = hpHienTai;
        nhanSt = GetComponent<Animator>();
    }

    void Update()
    {

        // Ghi chú: Mã dưới đây chỉ để kiểm tra (test) logic giảm máu bằng phím T.
        // Bạn có thể để mã này hoặc xóa nếu không cần test nữa
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    Debug.Log("Test sát thương bằng phím T");
        //    GiamMau(20);
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra nếu va chạm với hitbox
        // Sau này có thể thay đổi logic để xử lý va chạm với viên đạn hoặc player
        if (collision.gameObject.CompareTag("hitbox"))
        {
            GiamMau(20);
        }
    }

    // Hàm giảm máu từ bên ngoài (gọi từ Quai4)
    public void GiamMau(int soMau)
    {
        hpHienTai -= soMau;
        hp.value = hpHienTai;
        StartCoroutine(nhanST());

        if (hpHienTai <= 0)
        {
            Debug.Log("Quái đã chết.");
            //Destroy(gameObject);
        }

        // Nếu có tham chiếu đến script Quai4, giảm máu của Quai4
        if (quai4Script != null)
        {
            quai4Script.GiamMau(soMau);  // Giảm máu cho Quai4 từ Quai4_Hiep
        }
    }

    IEnumerator nhanST()
    {
        nhanSt.SetBool("nhanSt", true);
        yield return new WaitForSeconds(0.3f);
        nhanSt.SetBool("nhanSt", false);
    }
}
