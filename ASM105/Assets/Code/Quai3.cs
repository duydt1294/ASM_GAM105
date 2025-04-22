using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quai3 : MonoBehaviour
{
    public int mautoida = 100;
    public int mauhientai;
    [SerializeField] Slider hpQuai;
    Animator quai3,quai3chet;
    void Start()
    {
        mauhientai = mautoida;
        hpQuai.value = mauhientai;
        quai3 = GetComponent<Animator>();
        quai3chet = GetComponent<Animator>();

    }
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("hitbox"))
        {
            mauhientai -= 35;
            hpQuai.value = mauhientai;
            StartCoroutine(chayAnimation()); // Gọi hàm IEnumerator bên dưới để Bắt đầu animation
        }
        if(mauhientai <= 0)
        {
            quai3chet.SetTrigger("chet");
            Destroy(gameObject,1f);
        }
    }
    IEnumerator chayAnimation()
    {
        quai3.SetBool("NhanSt", true); // Dùng bool để kích hoạt animation
        yield return new WaitForSeconds(0.3f); // Đợi animation chạy trong 0.3s
        quai3.SetBool("NhanSt", false); // Tắt animation

    }

}
