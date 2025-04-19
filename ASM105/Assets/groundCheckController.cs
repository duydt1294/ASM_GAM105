using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheckController : MonoBehaviour
{
    private monster5Controller controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponentInParent<monster5Controller>();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("matDat"))
        {
            controller.kiemTraMatDat();
        }
    }

}
