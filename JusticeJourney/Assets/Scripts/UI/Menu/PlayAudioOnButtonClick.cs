using UnityEngine;
using UnityEngine.UI;

public class PlayAudioOnButtonClick : MonoBehaviour
{
    // Biến để lưu trữ tham chiếu đến thành phần Button
    Button _button;

    // Phương thức Awake được gọi khi script khởi tạo
    void Awake()
    {
        _button = GetComponent<Button>();
    }
    // Phương thức Start được gọi sau khi tất cả các phương thức Awake
    void Start()
    {
        // Thêm một Listener vào sự kiện onClick của nút
        _button.onClick.AddListener(() =>
        {
            // Khi nút được nhấn, gọi phương thức Play trong SoundManager
            // và truyền vào một kiểu enum SoundTags để xác định âm thanh cần phát
            SoundManager.Instance.Play(SoundManager.SoundTags.ButtonClick);
        });
    }
}
