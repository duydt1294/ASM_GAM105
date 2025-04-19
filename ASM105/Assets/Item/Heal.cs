using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
	public int healAmount = 50;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			//Tìm script PlayerHealth gắn trên Player
			PlayerController playerHealth = collision.GetComponent<PlayerController>(); //PlayerController la ten class script cua player

			if (playerHealth != null)
			{
				playerHealth.Heal(healAmount); // Tăng máu
				Destroy(gameObject);
			}
		}
	}
}

//Bổ sung vào code player
/*
public class PlayerHealth : MonoBehaviour
{
	public int maxHealth = 100;
	public int currentHealth;

	void Start()
	{
		currentHealth = maxHealth;
	}

	public void Heal(int amount)
	{
		currentHealth += amount;
		currentHealth = Mathf.Min(currentHealth, maxHealth); // Không vượt quá max
		Debug.Log("Hồi máu: " + amount + ". Máu hiện tại: " + currentHealth);
	}
}
*/