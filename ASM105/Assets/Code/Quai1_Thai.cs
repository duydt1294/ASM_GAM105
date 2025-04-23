using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    //CÁC THUỘC TÍNH TUẦN TRA 
    public float khoangCachDiChuyen = 2f;
    public float tocDoDiChuyen = 1f;
    private Vector3 viTriBatDau;
    private bool diChuyenSangPhai = true;
    //PHÁT HIỆN VÀ TẤN CÔNG NGƯỜI CHƠI 
    public Transform nguoiChoi;
    public float tamPhatHien = 5f;
    public float tocDoTanCong = 0.5f;
    public float khoangDungTanCong = 0.5f;
    public float tocDoKhiTruyDuoi = 5f;
    //QUẢN LÝ MÁU VÀ ANIMATION 
    public int maxHealth = 3;
    private int currentHealth;
    private Animator animator, tancong, dead;
    private bool daChet = false;
    public float cooldownTime = 1f; // Thời gian cooldown
    private float nextActionTime = 0f;
    void Start()
    {
        nguoiChoi = GameObject.FindGameObjectWithTag("Player").transform;
        viTriBatDau = transform.position;
        animator = GetComponent<Animator>();
        tancong = GetComponent<Animator>();
        dead = GetComponent<Animator>();
        currentHealth = maxHealth;
    }
    void Update()
    {
        if (daChet) return;
        float khoangCach = Vector2.Distance(transform.position, nguoiChoi.position);
        if (khoangCach < tamPhatHien)
        {
            TanCongNguoiChoi();
        }
        else
        {
            TuanTra();
        }
    }
    void TuanTra()
    {
        tancong.SetBool("Attack", false);
        animator.SetTrigger("run");

        if (diChuyenSangPhai)
        {
            transform.position += Vector3.right * tocDoDiChuyen * Time.deltaTime;

            if (transform.position.x >= viTriBatDau.x + khoangCachDiChuyen)
                diChuyenSangPhai = false;

            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.position += Vector3.left * tocDoDiChuyen * Time.deltaTime;

            if (transform.position.x <= viTriBatDau.x - khoangCachDiChuyen)
                diChuyenSangPhai = true;

            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    void TanCongNguoiChoi()
    {
        float khoangCachToiNguoiChoi = Vector2.Distance(transform.position, nguoiChoi.position);
        float tocDoDiChuyen = tocDoTanCong + tocDoKhiTruyDuoi; // ← tăng tốc truy đuổi

        if (khoangCachToiNguoiChoi > khoangDungTanCong)
        {
            transform.position = Vector2.MoveTowards(transform.position, nguoiChoi.position, tocDoDiChuyen * Time.deltaTime);
            XoayMatVePhiaNguoiChoi();
            tancong.SetBool("Attack", true);
            animator.SetTrigger("run");
        }
        else
        {
            animator.ResetTrigger("run");
            tancong.SetBool("Attack", false);
        }
    }
    void XoayMatVePhiaNguoiChoi()
    {
        if (nguoiChoi.position.x < transform.position.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }  
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, tamPhatHien);
    }
}