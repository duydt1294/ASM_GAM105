using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class Boss2 : MonoBehaviour
{
    public int mauToiDa = 100;
    private int mauHienTai;
    Animator heo, heochet, dichuyen, tancong, chieuu1;
    [SerializeField] Slider hp;
    [SerializeField] private Camera mayAnh; // Camera chính (Kéo thả trong Inspector)
    [SerializeField] private Transform nguoiChoi; // Người chơi (Kéo thả trong Inspector)
    private Vector3 viTriBanDau; // Lưu vị trí ban đầu của Boss
    public float tocDoDiChuyen = 2f; // Tốc độ di chuyển của Boss
    private bool latMat = true;
    public bool TanCongBoss = false;
    //Cường
    [SerializeField] GameObject Chieu1; // Chiêu 1 (Kéo vào từ Inspector)
    [SerializeField] float tocDoChieu1 = 20f; // Tốc độ bay của chiêu 1
    private bool dangBanChieu1 = false;

    void Start()
    {
        mauHienTai = mauToiDa;
        hp.maxValue = mauToiDa;
        hp.value = mauHienTai;
        heo = GetComponent<Animator>();
        heochet = GetComponent<Animator>();
        dichuyen = GetComponent<Animator>();
        tancong = GetComponent<Animator>();
        chieuu1 = GetComponent<Animator>();
        viTriBanDau = transform.position;
    }

    private void Update()
    {
        if (mauHienTai > 0) // điều kiện nếu máu = 0 thì boss sẽ bị Destroy nhưng trong lúc chờ Destroy thì bos sẽ đứng im ko chạy theo Player nữa
        {
            if (CameraThayBoss())
            {
                DiChuyenTheoNguoiChoi(); // Nếu camera nhìn thấy Boss, Boss sẽ di chuyển theo người chơi
            }
            else
            {
                QuayLaiViTriBanDau(); // Nếu camera không nhìn thấy Boss, Boss quay lại vị trí ban đầu
            }
        }
        else { }
    }
    private void OnTriggerEnter2D(Collider2D collison)
    {
        if (collison.gameObject.tag.Equals("hitbox"))
        {
            mauHienTai -= 30;
            hp.value = mauHienTai;

            StartCoroutine(ChayAnimation()); // Gọi hàm IEnumerator bên dưới để Bắt đầu animation
        }

        if (mauHienTai <= 0)
        {
            heochet.SetTrigger("chet");
            Destroy(gameObject, 2f);
        }

    }

    IEnumerator ChayAnimation()
    {
        heo.SetBool("nhanSt", true); // Dùng bool để kích hoạt animation
        yield return new WaitForSeconds(0.3f); // Đợi animation chạy trong 0.3s
        heo.SetBool("nhanSt", false); // Tắt animation
                                      // 

    }
    private bool CameraThayBoss()
    {
        if (mayAnh == null) return false; // Kiểm tra camera có bị null không

        // Chuyển vị trí Boss sang tọa độ màn hình (viewport)
        Vector3 viTriBossTrenManHinh = mayAnh.WorldToViewportPoint(transform.position);

        // Kiểm tra xem Boss có nằm trong khung hình không
        return viTriBossTrenManHinh.x >= 0 && viTriBossTrenManHinh.x <= 1 &&
               viTriBossTrenManHinh.y >= 0 && viTriBossTrenManHinh.y <= 1 &&
               viTriBossTrenManHinh.z > 0;
    }

    private void DiChuyenTheoNguoiChoi()
    {
        // Di chuyển Boss đến vị trí của người chơi
        transform.position = Vector3.MoveTowards(transform.position, nguoiChoi.position, tocDoDiChuyen * Time.deltaTime);

        dichuyen.SetBool("run", true);

        if (nguoiChoi.position.x > transform.position.x && latMat)
        {
            Flip();
        }
        if (nguoiChoi.position.x < transform.position.x && !latMat)
        {
            Flip();
        }
        if (mauHienTai <= 50 && !TanCongBoss)
        {
            StartCoroutine(TanCong());
        }
        if (mauHienTai <= 100 && !dangBanChieu1)
        {
            StartCoroutine(BanChieu1());
        }
    }

    private void QuayLaiViTriBanDau()
    {
        // Di chuyển Boss về vị trí ban đầu
        transform.position = Vector3.MoveTowards(transform.position, viTriBanDau, tocDoDiChuyen * Time.deltaTime);
        dichuyen.SetBool("run", false);
    }
    void Flip()
    {
        latMat = !latMat;
        Vector3 latLai = transform.localScale;
        latLai.x *= -1;
        transform.localScale = latLai;
    }
    IEnumerator TanCong() // chiêu 2 là nuốt thằng Player vào bụng 
    {
        while (mauHienTai <= 50)
        {
            float tocDoLuot = 10f; // tốc độ lướt

            while (mauHienTai <= 50)
            {
                TanCongBoss = true;
                tancong.SetBool("tancong", true);

                // Tính hướng lướt (tới người chơi)
                Vector3 huongLuot = (nguoiChoi.position - transform.position).normalized;

                // Lướt tới một khoảng ngắn
                float khoangCachLuot = 5f; // khoảng cách boss lướt
                Vector3 viTriDich = transform.position + huongLuot * khoangCachLuot;

                float thoiGianLuot = 0.2f; // thời gian lướt
                float thoiGianDem = 0f;

                Vector3 viTriBatDau = transform.position;

                while (thoiGianDem < thoiGianLuot)
                {
                    transform.position = Vector3.Lerp(viTriBatDau, viTriDich, thoiGianDem / thoiGianLuot);
                    thoiGianDem += Time.deltaTime * tocDoLuot;
                    yield return null;
                }
                // Kết thúc animation tấn công
                tancong.SetBool("tancong", false);
                yield return new WaitForSeconds(5f);
                TanCongBoss = false;
            }
        }
    }
    IEnumerator BanChieu1()
    {
        dangBanChieu1 = true;

        while (mauHienTai > 0) // Boss còn sống mới bắn
        {
            chieuu1.SetBool("chieu1", true);
            yield return new WaitForSeconds(0.5f);
            chieuu1.SetBool("chieu1", false);
            // Hướng bắn từ Boss tới người chơi
            Vector2 huongBan = (nguoiChoi.position - transform.position).normalized;

            // Tạo viên đạn cách Boss một chút theo hướng bắn
            Vector3 viTriSpawn = transform.position + (Vector3)huongBan * 1f; // 1f là khoảng cách đẩy ra
            
            GameObject dan = Instantiate(Chieu1, viTriSpawn, Quaternion.identity);
            Rigidbody2D rb = dan.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = huongBan * tocDoChieu1;
            }

            Destroy(dan, 4f); // Viên đạn tự hủy sau 4 giây
            yield return new WaitForSeconds(4f); // Chờ 4 giây trước khi bắn viên tiếp theo
        }

        dangBanChieu1 = false;
    }
}
