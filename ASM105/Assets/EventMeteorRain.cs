using UnityEngine;

public class MeteorRainSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;  // Thiên thạch prefab (thiên thạch sẽ được spawn)
    public Vector3 spawnMin = new Vector3(107.99f, 8.1f, 0f);  // Tọa độ tối thiểu (X=107.99, Y=8.1, Z=0)
    public Vector3 spawnMax = new Vector3(126.87f, 8.1f, 0f);  // Tọa độ tối đa (X=126.87, Y=8.1, Z=0)

    public float spawnInterval = 0.8f;  // Thời gian giữa các lần spawn (tính bằng giây)

    void Start()
    {
        InvokeRepeating("SpawnMeteor", 0f, spawnInterval);
    }

    void SpawnMeteor()
    {
        for (int i = 0; i < 8; i++)
        {
            // Tạo một vị trí spawn ngẫu nhiên trong phạm vi đã cho
            float randomX = Random.Range(spawnMin.x, spawnMax.x);
            float randomY = spawnMin.y;  // Y sẽ giữ nguyên vì bạn đã chỉ định y = 8.1
            float randomZ = spawnMin.z;  // Z giữ nguyên nếu bạn không cần thay đổi Z

            // Tạo vị trí ngẫu nhiên
            Vector3 spawnPosition = new Vector3(randomX, randomY, randomZ);

            // Spawn thiên thạch
            GameObject meteor = Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);

            // Lấy Rigidbody2D của thiên thạch để thay đổi tốc độ
            Rigidbody2D rb = meteor.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Tăng tốc độ di chuyển nhanh hơn
                rb.velocity = new Vector2(Random.Range(-5f, 5f), -10f);  // Tăng tốc độ di chuyển X và Y (bay nhanh hơn)
            }
        }
    }
}
