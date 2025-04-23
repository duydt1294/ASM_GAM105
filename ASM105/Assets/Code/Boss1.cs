using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss1 : MonoBehaviour
{
    int mautoida = 100;
    int mauhientai;
    [SerializeField] Slider hp;
    Animator boss1bidanh,chet;
    public bool maudahoi = false;
    void Start()
    {
        mauhientai = mautoida;
        hp.value = mauhientai;
        boss1bidanh = GetComponent<Animator>();
        chet = GetComponent<Animator>();
    }
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("hitbox"))
        {
            mauhientai -= 10;
            hp.value = mauhientai;
            StartCoroutine(chayAnimation());
        }
        if(mauhientai <= 0)
        {
            chet.SetTrigger("chet");
            Destroy(gameObject,1.5f);
        }
    }
    IEnumerator chayAnimation()
    {
        boss1bidanh.SetBool("bidanh", true); // Dùng bool để kích hoạt animation
        yield return new WaitForSeconds(0.3f);
        boss1bidanh.SetBool("bidanh", false); // Tắt animation

    }
}
