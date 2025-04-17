using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    public float khoangCachDiChuyen = 2f;        // Khoảng cách tối đa di chuyển từ vị trí ban đầu
    public float tocDoDiChuyen = 1f;             // Tốc độ di chuyển tuần tra
    private Vector3 viTriBatDau;                 // Vị trí ban đầu của Thợ Săn
    private bool diChuyenSangPhai = true;        // Đang đi sang phải hay không

    public Transform nguoiChoi;                  // Đối tượng Player
    public float tamPhatHien = 5f;               // Phạm vi phát hiện người chơi
    public float tocDoTanCong = 2f;              // Tốc độ di chuyển khi tấn công
    public float khoangDungTanCong = 0.5f;       // Khoảng cách để dừng lại và tấn công

    void Start()
    {
        viTriBatDau = transform.position; // Ghi lại vị trí ban đầu
    }
    void Update()
    {
        float khoangCach = Vector2.Distance(transform.position, nguoiChoi.position);

        if (khoangCach < tamPhatHien)
        {
            TanCongNguoiChoi(); // Nếu thấy Player thì tấn công
        }
        else
        {
            TuanTra(); // Nếu không thấy thì tuần tra
        }
    }
    // Di chuyển qua lại khi không phát hiện người chơi
    void TuanTra()
    {
        if (diChuyenSangPhai)
        {
            transform.position += Vector3.right * tocDoDiChuyen * Time.deltaTime;

            if (transform.position.x >= viTriBatDau.x + khoangCachDiChuyen)
                diChuyenSangPhai = false;

            transform.rotation = Quaternion.Euler(0, 0, 0); // Quay mặt phải
        }
        else
        {
            transform.position += Vector3.left * tocDoDiChuyen * Time.deltaTime;

            if (transform.position.x <= viTriBatDau.x - khoangCachDiChuyen)
                diChuyenSangPhai = true;

            transform.rotation = Quaternion.Euler(0, 180, 0); // Quay mặt trái
        }
    }
    // Tấn công người chơi
    void TanCongNguoiChoi()
    {
        float khoangCachToiNguoiChoi = Vector2.Distance(transform.position, nguoiChoi.position);

        if (khoangCachToiNguoiChoi > khoangDungTanCong)
        {
            // Di chuyển lại gần người chơi
            transform.position = Vector2.MoveTowards(transform.position, nguoiChoi.position, tocDoTanCong * Time.deltaTime);

            // Xoay mặt về hướng người chơi
            XoayMatVePhiaNguoiChoi();
        }
        else
        {
            // Đã gần → có thể thêm animation tấn công ở đây
        }
    }
    // Xoay mặt theo hướng của người chơi
    void XoayMatVePhiaNguoiChoi()
    {
        if (nguoiChoi.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // Quay trái
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Quay phải
        }
    }
    private void OnDrawGizmosSelected()
    {
        // Vẽ phạm vi phát hiện trong Scene
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, tamPhatHien);
    }
}