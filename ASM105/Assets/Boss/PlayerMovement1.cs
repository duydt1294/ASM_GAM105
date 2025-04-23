using UnityEngine;
using System.Collections; // Thêm dòng này để sử dụng IEnumerator

public class PlayerMovement1 : MonoBehaviour
{
    public float speed = 5f;
    private float originalSpeed;

    void Start()
    {
        originalSpeed = speed; // Lưu tốc độ ban đầu của người chơi
    }

    public void ApplySlow(float slowAmount, float duration)
    {   
        MainMoveMent playerMoveMent = GetComponent<MainMoveMent>();
        playerMoveMent.speed *= (1f - slowAmount); // Giảm tốc độ
        StartCoroutine(ResetSpeed(duration)); // Khôi phục tốc độ sau thời gian làm chậm
    }

    private IEnumerator ResetSpeed(float duration)
    {
        yield return new WaitForSeconds(duration);
        MainMoveMent playerMoveMent = GetComponent<MainMoveMent>();
        playerMoveMent.speed = originalSpeed; // Khôi phục tốc độ ban đầu
    }

    void Update()
    {
        // Di chuyển người chơi (Thêm điều khiển di chuyển của bạn ở đây)
    }
}
