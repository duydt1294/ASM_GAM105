using UnityEngine;

public class PortalTeleporter2 : MonoBehaviour
{
    public Vector3 teleportPosition2 = new Vector3(92.3f, -83.6f, 0f); // Vị trí cần dịch chuyển

    // Kiểm tra khi có va chạm
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem đối tượng va chạm có tag "Player" không
        if (other.CompareTag("Player"))
        {
            // Dịch chuyển đối tượng "Player" đến vị trí mới
            other.transform.position = teleportPosition2;
        }
    }
}
