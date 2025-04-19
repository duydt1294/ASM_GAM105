using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    public Vector3 teleportPosition = new Vector3(-28.9f, -79f, 0f); // Vị trí cần dịch chuyển

    // Kiểm tra khi có va chạm
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem đối tượng va chạm có tag "Player" không
        if (other.CompareTag("Player"))
        {
            // Dịch chuyển đối tượng "Player" đến vị trí mới
            other.transform.position = teleportPosition;
        }
    }
}
