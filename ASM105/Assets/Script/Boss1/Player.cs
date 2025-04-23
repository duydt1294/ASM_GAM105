using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public int health = 100;
    public float moveSpeed = 5f;
    private float originalSpeed;

    private void Start()
    {
        originalSpeed = moveSpeed;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log("Player bị trúng đòn! Mất " + amount + " máu. Máu còn lại: " + health);
    }

    public void ApplySlow(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(SlowEffect(duration));
    }

    private System.Collections.IEnumerator SlowEffect(float duration)
    {
        moveSpeed *= 0.5f;
        Debug.Log("Player bị làm chậm");
        yield return new WaitForSeconds(duration);
        moveSpeed = originalSpeed;
        Debug.Log("Player hết bị làm chậm");
    }
    public void ApplyPoison(float duration, float percentPerSecond)
    {
        StartCoroutine(PoisonEffect(duration, percentPerSecond));
    }

    private IEnumerator PoisonEffect(float duration, float dps)
    {
        float timer = 0f;
        while (timer < duration)
        {
            TakeDamage(Mathf.RoundToInt(health * dps));
            yield return new WaitForSeconds(1f);
            timer += 1f;
        }
    }
}
