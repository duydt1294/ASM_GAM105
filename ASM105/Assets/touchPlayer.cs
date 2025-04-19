using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class touchPlayer : MonoBehaviour
{
    private monster5Controller controller;

    void Start()
    {
        controller = GetComponentInParent<monster5Controller>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
            if (other.CompareTag("Player"))
            {
                controller.TanCongPlayer();
            }
    }
}

