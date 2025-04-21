using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public float speed = 1f; // Tốc độ di chuyển
    public float leftLimit = 2.21f; // Giới hạn bên trái
    public float rightLimit = 6.04f; // Giới hạn bên phải
    private bool movingRight = true; // Hướng di chuyển (true = phải)

    public float PhamViPhatHienNhanVat = 2f; //Phạm vi mà quái bắt đầu rượt theo nhân vật
    private float speedGoc; // Lưu tốc độ
    public float speedRuotDuoi = 2f; // Tốc độ quái rượt nhân vật

    private SpriteRenderer spriteRenderer; // Để lật hình quái
    private Transform player; // Lưu vị trí nhân vật

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Lấy SpriteRenderer từ quái
        player = GameObject.FindGameObjectWithTag("Player").transform; // tìm nhân vật có tag player
        speedGoc = speed; // Ghi nhớ tốc độ gốc
    }

    void Update()
    {
        // Tính khoảng cách giữa quái và nhân vật
        float KhoangCachToiNhanVat = Vector2.Distance(transform.position, player.position);

        if(KhoangCachToiNhanVat < PhamViPhatHienNhanVat)  // Nếu người chơi nằm trong phạm vi phát hiện → rượt đuổi
        {
            DuoiTheoNhanVat();
        }
        else // Nếu không thấy người chơi → tuần tra như bình thường
        {
            TuanTra();
        }
        // Quái tuần tra trong phạm vi giới hạn
        void TuanTra()
        {   
            speed = speedGoc; // Trả lại tốc độ gốc khi tuần tra
            // Di chuyển quái
            if (movingRight)
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                
                if (transform.position.x >= rightLimit)
                // transform.position.x: Lấy vị trí hiện tại của quái trên trục X.
                {
                    movingRight = false; // Đổi hướng sang trái
                    FlipSprite(); // Lật hình
                }
            }
            else
            {
                transform.Translate(Vector2.left * speed * Time.deltaTime);
                //transform.Translate(...) = Di chuyển đối tượng theo hướng cụ thể, dựa trên vị trí hiện tại.
                if (transform.position.x <= leftLimit)
                {
                    movingRight = true; // Đổi hướng sang phải
                    FlipSprite(); // Lật hình
                }
            }
        }
        // Hàm rượt đuổi người chơi
        void DuoiTheoNhanVat()
        {   
            speed = speedRuotDuoi; // Tăng tốc độ khi rượt đuổi
            Vector2 PhuongHuong;
            if(player.position.x > transform.position.x)
            // Nếu player.x lớn hơn quái.x → tức là nhân vật đang ở bên phải quái.
            {
                PhuongHuong = Vector2.right; // Vậy hướng rượt sẽ là: Vector2.right → đi sang phải.
                
                if (!movingRight) // Nếu quái đang không nhìn sang phải (đang nhìn trái)
                {
                    movingRight = true; // Quay lại hướng phải
                    FlipSprite(); // Lật sprite để quay mặt qua phải
                }
            }
            else //Nếu player.x < quái.x → quái phải rượt sang trái.
            {
                PhuongHuong = Vector2.left;
                // Vì nhân vật đang ở bên trái (x nhỏ hơn), nên quái phải đi sang trái
                if (movingRight)  // Nếu quái đang nhìn phải, mà giờ phải quay trái
                {
                    movingRight = false; // Quay lại hướng trái
                    FlipSprite();// Lật sprite để quay mặt qua trái
                }
            }
            transform.Translate(PhuongHuong * speed * Time.deltaTime);
        }
       
    }

    // Hàm lật hình quái
    void FlipSprite()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
    // Hiển thị phạm vi phát hiện người chơi trong Scene view (chỉ để debug)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PhamViPhatHienNhanVat);
    }

}
