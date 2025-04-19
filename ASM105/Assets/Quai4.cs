using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quai4 : MonoBehaviour
{
    public int mautoida = 100;
    public int mauhientai;
    [SerializeField] Slider hpQuai;
    Animator quai4, quai4chet;
    void Start()
    {
        mauhientai = mautoida;
        hpQuai.value = mauhientai;
        quai4 = GetComponent<Animator>();
        quai4chet = GetComponent<Animator>();

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
            StartCoroutine(chayAnimation()); // G?i hàm IEnumerator bên d??i ?? B?t ??u animation
        }
        if (mauhientai <= 0)
        {
            quai4chet.SetTrigger("chet");
            Destroy(gameObject, 1f);
        }
    }
    IEnumerator chayAnimation()
    {
        quai4.SetBool("NhanSt", true); // Dùng bool ?? kích ho?t animation
        yield return new WaitForSeconds(0.3f); // ??i animation ch?y trong 0.3s
        quai4.SetBool("NhanSt", false); // T?t animation

    }

}
