using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator run;
    float speed = 5f;
    bool latmat;
    public float lucLuot = 20f;      // lực lướt
    public float thoiGianLuot = 0.5f; // thời gian lướt
    bool dangLuot = false;
    float demNguocLuot = 0f;
    float hoiLuot = 1f;          // thời gian hồi
    float demNguocHoiLuot = 0f;  // đếm ngược hồi lướt

    Vector3 huongLuot;
    void Start()
    {
        run = GetComponent<Animator>();
    }

    void Update()
    {
        float trucX = 0f, trucZ = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            trucX = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            trucX = 1;
        }
        Vector3 dichuyen = new Vector3(trucX, trucZ).normalized;
        transform.position += dichuyen * speed * Time.deltaTime;


        if (trucX > 0 && latmat)
        {
            flip();
        }
        if (trucX < 0 && !latmat)
        {
            flip();
        }
        run.SetBool("run", trucX != 0);

        if (dangLuot)
        {
            transform.position += huongLuot * lucLuot * Time.deltaTime;
            demNguocLuot -= Time.deltaTime;
            if (demNguocLuot <= 0)
            {
                dangLuot = false;
            }
        }
        else
        {
            // Bình thường đi bộ
            transform.position += dichuyen * speed * Time.deltaTime;

            // Bấm Shift để lướt
            if (Input.GetKeyDown(KeyCode.LeftShift) && trucX != 0 && demNguocHoiLuot <= 0)
            {
                dangLuot = true;
                demNguocLuot = thoiGianLuot;
                huongLuot = dichuyen; // hướng đang di chuyển
                demNguocHoiLuot = hoiLuot;
            }
            if (demNguocHoiLuot > 0)
            {
                demNguocHoiLuot -= Time.deltaTime;
            }
        }
    }
    void flip()
    {
        latmat = !latmat;
        Vector3 nv = transform.localScale;
        nv.x *= -1;
        transform.localScale = nv;
    }
}
