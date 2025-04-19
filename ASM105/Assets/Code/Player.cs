using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    AudioSource Luot;
    Animator run, chem, luot,trungchieu2;
    float speed = 5f;
    bool latmat;
    public float lucLuot = 20f;      // lực lướt
    public float thoiGianLuot = 0.5f; // thời gian lướt
    bool dangLuot = false;
    float demNguocLuot = 0f;
    float hoiLuot = 1f;          // thời gian hồi
    float demNguocHoiLuot = 0f;  // đếm ngược hồi lướt
    int HpPlayer = 100;
    int HpHienTai;
    [SerializeField] Slider hp;
    Vector3 huongLuot;
    bool biKhongChe = false; // Không thể di chuyển khi true




    //private Rigidbody2D vatLy;
    //public float lucNhay = 12f; // Lực nhảy
    //private bool trenMatDat;


    void Start()
    {
        Luot = GameObject.Find("Luot").GetComponent<AudioSource>();
        run = GetComponent<Animator>();
        chem = GetComponent<Animator>();
        luot = GetComponent<Animator>();
        trungchieu2 = GetComponent<Animator>();
        HpHienTai = HpPlayer;
        hp.value = HpHienTai;
        hp.value = HpPlayer;
        //vatLy = GetComponent<Rigidbody2D>();
        
    }

    void Update()
    {
        if (biKhongChe) return;


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
                
                luot.SetBool("Luot", true);
                dangLuot = true;
                demNguocLuot = thoiGianLuot;
                huongLuot = dichuyen; // hướng đang di chuyển
                demNguocHoiLuot = hoiLuot;
                Luot.Play();
            }
            else
            {
                luot.SetBool("Luot", false);
            }
            if (demNguocHoiLuot > 0)
            {
                demNguocHoiLuot -= Time.deltaTime;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            chem.SetBool("Attack", true);
        }
        else
        {
            chem.SetBool("Attack", false);
        }
        //Lật Flip
        if (trucX > 0 && latmat)
        {
            flip();
        }
        if (trucX < 0 && !latmat)
        {
            flip();
        }
        run.SetBool("run", trucX != 0);
    }
    void flip()
    {
        latmat = !latmat;
        Vector3 nv = transform.localScale;
        nv.x *= -1;
        transform.localScale = nv;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            HpHienTai -= 10;
            hp.value = HpHienTai;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("chieu2"))
        {
            HpHienTai -= 10;
            hp.value = HpHienTai;
            StartCoroutine(TrungChieuBoss2());
        }   
    }
    IEnumerator TrungChieuBoss2() // Bị trúng chiêu 2 của Boss nuốt vào bụng
    {
        trungchieu2.SetBool("trungchieu2", true); // chạy Animation Player bị Boss 2 nuốt khi trúng chiêu
        biKhongChe = true; // Vô hiệu hóa để người chơi ko điều khiển được Player khi trúng chiêu
        yield return new WaitForSeconds(0.6f);
        trungchieu2.SetBool("trungchieu2", false);

        GetComponent<SpriteRenderer>().enabled = false; // ẩn Player đi trong 1s
        yield return new WaitForSeconds(1f); 
        GetComponent<SpriteRenderer>().enabled = true; // hiện lại Player
        biKhongChe = false; // bỏ vô hiệu hóa để người chơi có thể điều khiển thằng Player lại
    }
}
