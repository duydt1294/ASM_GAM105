using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nhanSatThuongController : MonoBehaviour
{
    private monster5Controller controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponentInParent<monster5Controller>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("bullet"))
        {
            {
                controller.kiemTraSatThuong();
            }

        }
    }
}
