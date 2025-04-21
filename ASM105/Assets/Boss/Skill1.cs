using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1 : MonoBehaviour
{
    public GameObject SkillPrefab; // là Prefab bóng tối (miệng đói) mà bạn đã tạo sẵn. Đây là thứ sẽ được sinh ra khi boss dùng chiêu.
    public float damage = 10f; // Sát thương
    public float slowDuration = 3f; // Thời gian làm chậm (3 giây)
    public float slowAmount = 0.5f; // Tỷ lệ làm chậm (50%)

    public float cooldownTime = 5f; // Thời gian cooldown giữa các lần sử dụng chiêu thức (5 giây)
    private float nextTimeCanUseSkill = 0f; // Thời gian boss có thể sử dụng chiêu thức lại

    private bool isSkillActive = false; // Kiểm tra xem chiêu thức đã được gọi hay chưa

    private GameObject currentShadow; // lưu lại bóng tối đã sinh ra, giúp đảm bảo chỉ có một bóng tối tồn tại tại một thời điểm

    

    // Update is called once per frame
    void Update()
    {
        // Kiểm tra xem boss có thể sử dụng chiêu thức lại không (theo cooldown) và chiêu thức chưa được gọi
        if (Time.time >= nextTimeCanUseSkill && isSkillActive == false && currentShadow == null)
        {
            // Nếu không có cooldown và chiêu thức chưa được gọi, gọi chiêu thức
            StartCoroutine(UseSkill());
        }
    }

    IEnumerator UseSkill()
    {
        // Đánh dấu chiêu thức là đang được sử dụng
        isSkillActive = true;

        // Tạo bóng tối hoặc miệng đói từ sau lưng (chỉ khi không có bóng tối nào đang tồn tại)
        currentShadow = Instantiate(SkillPrefab, transform.position, Quaternion.identity);

        // Lấy Rigidbody2D của bóng tối để điều khiển di chuyển
        Rigidbody2D rb = currentShadow.GetComponent<Rigidbody2D>();

        // Tìm vị trí của người chơi (có tag "Player")
        Vector3 playerPosition = GameObject.FindWithTag("Player").transform.position;

        // Tính toán hướng di chuyển từ bóng tối tới người chơi
        Vector3 direction = (playerPosition - currentShadow.transform.position).normalized;

        // Di chuyển bóng tối về phía người chơi với tốc độ 5f
        rb.velocity = direction * 5f; // Điều chỉnh tốc độ di chuyển của bóng tối

        // Thêm component để xử lý va chạm
        ShadowCollision shadowCollision = currentShadow.AddComponent<ShadowCollision>(); // Gắn script ShadowCollision vào bóng tối
        shadowCollision.damage = damage; // Gán sát thương (được khai báo ở trên) vào script xử lý va chạm
        shadowCollision.slowDuration = slowDuration; // Gán thời gian làm chậm vào script xử lý va chạm
        shadowCollision.slowAmount = slowAmount; // Gán tỷ lệ làm chậm vào script xử lý va chạm

        // Hủy bóng tối sau một khoảng thời gian (ví dụ: 2 giây) để không sinh quá nhiều bóng tối
        Destroy(currentShadow, 2f); // Bóng tối tự động biến mất sau 2 giây

        // Đặt lại thời gian có thể sử dụng chiêu thức (cooldown)
        nextTimeCanUseSkill = Time.time + cooldownTime;

        // Chờ đến khi cooldown kết thúc trước khi cho phép chiêu thức được sử dụng lại
        yield return new WaitForSeconds(cooldownTime);

        // Sau khi cooldown xong, cho phép chiêu thức có thể được kích hoạt lại
        isSkillActive = false;
    }
}
