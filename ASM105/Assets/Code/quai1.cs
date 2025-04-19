using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class quai1 : MonoBehaviour
{
    [SerializeField] Slider hp;
    int mautoida = 100;
    int mauhientai;
    Animator quaiBiDanh,chet;
    void Start()
    {
        mauhientai = mautoida;
        hp.value = mauhientai;
        quaiBiDanh = GetComponent<Animator>();
        chet = GetComponent<Animator>();
    }
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("hitbox"))
        {
            mauhientai -= 20;
            hp.value = mauhientai;
            StartCoroutine(chayAnimation());

        }
        if(mauhientai <= 0)
        {
            chet.SetTrigger("Dead");
            Destroy(gameObject,1f);
        }
    }
    IEnumerator chayAnimation() // Tạo hàm này để làm Animation khi quái bị nhận sát thương
    {
        quaiBiDanh.SetBool("Damege", true);
        yield return new WaitForSeconds(0.3f);
        quaiBiDanh.SetBool("Damege", false);
    }
}
