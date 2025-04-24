using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScene : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] AudioSource camAudio;
    private AudioSource bossMusic;
    // Start is called before the first frame update
    void Start()
    {
        door.SetActive(false);
        bossMusic = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            door.SetActive(true);
            camAudio.Stop();
            bossMusic.Play();
        }
    }
}
