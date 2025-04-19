using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quai1 : MonoBehaviour
{
    public Transform nguoiChoi;             // Player
    public float tocDoDiChuyen = 2f;        // Tốc độ di chuyển của Hunter
    public float tamTanCong = 1.5f;         // Khoảng cách để tấn công
    public float thoiGianHoiChieu = 1.0f;   // Thời gian giữa mỗi lần tấn công

    private Animator trinhDieuKhien;        // Animator của Hunter
    private float lanTanCongTruoc;          // Thời gian tấn công trước đó

    void Start()
    {
        trinhDieuKhien = GetComponent<Animator>();
    }

    void Update()
    {
        if (nguoiChoi == null) return;

        float khoangCach = Vector2.Distance(transform.position, nguoiChoi.position);

        if (khoangCach > tamTanCong)
        {
            // Di chuyển về phía người chơi
            Vector2 huong = (nguoiChoi.position - transform.position).normalized;
            transform.position += (Vector3)huong * tocDoDiChuyen * Time.deltaTime;

            // Bạn có thể thêm animation "chạy" ở đây nếu muốn
        }
        else
        {
            // Nếu đủ gần thì tấn công (sau thời gian hồi chiêu)
            if (Time.time - lanTanCongTruoc >= thoiGianHoiChieu)
            {
                trinhDieuKhien.SetTrigger("Attack");
                lanTanCongTruoc = Time.time;

                // Gọi hàm gây sát thương cho người chơi ở đây nếu cần
            }
        }

        // Lật hướng Hunter nếu người chơi ở bên trái/phải (cho game 2D ngang)
        if (nguoiChoi.position.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1); // Quay sang trái
        else
            transform.localScale = new Vector3(1, 1, 1); // Quay sang phải
    }
}