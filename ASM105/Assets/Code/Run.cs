using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Run : MonoBehaviour
{
    // ----------------- CÁC THUỘC TÍNH TUẦN TRA -----------------
    //Lê Hoàng Thái: di chuyển tuần tra, truy đuổi người chơi
    //Nguyễn Thành Lợi: chết
    //Đăng Khoa: tấn công 
    public float khoangCachDiChuyen = 2f;      // Khoảng cách tuần tra từ vị trí ban đầu
    public float tocDoDiChuyen = 1f;           // Tốc độ đi tuần tra
    private Vector3 viTriBatDau;               // Vị trí ban đầu
    private bool diChuyenSangPhai = true;      // Hướng di chuyển hiện tại

    // ----------------- PHÁT HIỆN VÀ TẤN CÔNG NGƯỜI CHƠI -----------------

    public Transform nguoiChoi;                // Tham chiếu đến player
    public float tamPhatHien = 5f;             // Phạm vi để bắt đầu đuổi theo
    public float tocDoTanCong = 2f;            // Tốc độ di chuyển khi đuổi player
    public float khoangDungTanCong = 0.5f;     // Khoảng cách gần để bắt đầu animation tấn công

    // ----------------- QUẢN LÝ MÁU VÀ ANIMATION -----------------
    //public int maxHealth = 3;                  // Máu tối đa
    //private int currentHealth;                 // Máu hiện tại

    private Animator animator, attack;                 // Animator để điều khiển animation
    /*private bool daChet = false;  */             // Kiểm tra đã chết hay chưa

    // ----------------- KHỞI TẠO -----------------
    void Start()
    {
        nguoiChoi = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        viTriBatDau = transform.position;      // Ghi lại vị trí ban đầu
        animator = GetComponent<Animator>(); // Lấy Animator từ đối tượng
        attack = GetComponent<Animator>();
        // Lấy Animator từ đối tượng
        /*currentHealth = maxHealth;  */           // Gán máu hiện tại
    }

    // ----------------- VÒNG LẶP CHÍNH -----------------
    void Update()
    {
        // Nếu đã chết thì không làm gì nữa
        //if (daChet) return;

        // Tính khoảng cách giữa hunter và người chơi
        float khoangCach = Vector2.Distance(transform.position, nguoiChoi.position);

        // Nếu phát hiện người chơi trong phạm vi
        if (khoangCach < tamPhatHien)
        {
            TanCongNguoiChoi();
        }
        else
        {
            TuanTra();
        }
    }

    // ----------------- DI CHUYỂN TUẦN TRA -----------------
    void TuanTra()
    {
        if (diChuyenSangPhai)
        {
            animator.SetTrigger("run");
            // Di chuyển sang phải
            transform.position += Vector3.right * tocDoDiChuyen * Time.deltaTime;

            // Nếu đã đi hết khoảng cách cho phép thì đổi hướng
            if (transform.position.x >= viTriBatDau.x + khoangCachDiChuyen)
                diChuyenSangPhai = false;

            // Xoay mặt sang phải
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            // Di chuyển sang trái
            transform.position += Vector3.left * tocDoDiChuyen * Time.deltaTime;

            // Nếu đã đi hết khoảng cách cho phép thì đổi hướng
            if (transform.position.x <= viTriBatDau.x - khoangCachDiChuyen)
                diChuyenSangPhai = true;

            // Xoay mặt sang trái
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    // ----------------- HÀNH VI TẤN CÔNG NGƯỜI CHƠI -----------------
    void TanCongNguoiChoi()
    {
        float khoangCachToiNguoiChoi = Vector2.Distance(transform.position, nguoiChoi.position);

        // Nếu còn xa → tiếp tục đuổi
        if (khoangCachToiNguoiChoi > khoangDungTanCong)
        {
            // Di chuyển lại gần người chơi
            transform.position = Vector2.MoveTowards(transform.position, nguoiChoi.position, tocDoTanCong * Time.deltaTime);

            // Xoay mặt về hướng người chơi
            XoayMatVePhiaNguoiChoi();

            // Phát animation chạy nếu chưa đủ gần để tấn công
            /*attack.ResetTrigger("Attack");*/ // Ngưng tấn công
            animator.SetTrigger("run");
        }
        else
        {
            // Gần đủ để tấn công → phát animation "attack"
            animator.ResetTrigger("run"); // Ngưng chạy
            //attack.SetTrigger("Attack");
        }
    }

    // ----------------- XOAY MẶT THEO HƯỚNG NGƯỜI CHƠI -----------------
    void XoayMatVePhiaNguoiChoi()
    {
        if (nguoiChoi.position.x < transform.position.x)
            transform.rotation = Quaternion.Euler(0, 0, 0); // Quay mặt trái
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);   // Quay mặt phải
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            attack.SetBool("Attack", true);
        }
    }
    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            attack.SetBool("Attack", false);
        }
    }


    // ----------------- BỊ NGƯỜI CHƠI TẤN CÔNG -----------------
    //public void BiTanCong(int satThuong)
    //{
    //    if (daChet) return;

    //    // Trừ máu
    //    currentHealth -= satThuong;

    //    // Phát animation "hurt"
    //    animator.SetTrigger("hurt");

    //    // Nếu máu <= 0 thì chết
    //    if (currentHealth <= 0)
    //    {
    //        Chet();
    //    }
    //}

    // ----------------- CHẾT -----------------
    //void Chet()
    //{
    //    daChet = true;

    //    // Phát animation "dead"
    //    animator.SetTrigger("dead");

    //    // Tắt collider để không va chạm nữa
    //    Collider2D col = GetComponent<Collider2D>();
    //    if (col != null) col.enabled = false;

    //    // Chờ vài giây rồi biến mất
    //    StartCoroutine(BienMat());
    //}

    // ----------------- BIẾN MẤT SAU KHI CHẾT -----------------
    //IEnumerator BienMat()
    //{
    //    yield return new WaitForSeconds(2f);
    //    Destroy(gameObject); // Xóa khỏi scene
    //}

    // ----------------- VẼ VÙNG PHÁT HIỆN TRONG SCENE -----------------
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, tamPhatHien);
    }
}