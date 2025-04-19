using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quai4_Hiep : MonoBehaviour
{
    Animator nhanSt;
    [SerializeField] Slider hp;
    public int hpHienTai;
    public int hpToiDa = 100;
    // Start is called before the first frame update
    void Start()
    {
        hpHienTai = hpToiDa;
        hp.value = hpHienTai;
        nhanSt = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("hitbox"))
        {
            hpHienTai -= 30;
            hp.value = hpHienTai;
            StartCoroutine(nhanST());
        }
        if (hpHienTai <= 0)
        {
            Destroy(gameObject);
        }
    }
    IEnumerator nhanST()
    {
        nhanSt.SetBool("nhanSt", true);
        yield return new WaitForSeconds(0.3f);
        nhanSt.SetBool("nhanSt", false);
    }
}
