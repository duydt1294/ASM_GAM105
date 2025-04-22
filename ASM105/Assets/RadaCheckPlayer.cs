using UnityEngine;

public class Playercheck : MonoBehaviour
{
    private monster5Controller controller;
    void Start()
    {
        controller = GetComponentInParent<monster5Controller>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            controller.PhatHienPlayer();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            controller.MatDauPlayer();
        }
    }
}
