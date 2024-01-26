using System;
using UnityEngine;
using UnityEngine.Audio;

// [Serializable] là một thuộc tính chỉ ra rằng đối tượng này có thể được serialize (lưu trữ dữ liệu) trong trình soạn thảo Unity.
[Serializable]
public class Sound
{
    // Tên của âm thanh, được sử dụng để định danh âm thanh trong hệ thống quản lý âm thanh (SoundManager).
    public SoundManager.SoundTags name;

    // Loại của âm thanh, có thể là âm thanh chung, âm thanh của người chơi, hoặc các loại khác tùy thuộc vào cách bạn tự định nghĩa.
    public SoundManager.SoundTypes type;

    // AudioClip chứa dữ liệu âm thanh thực tế.
    public AudioClip clip;

    // Độ lớn của âm thanh, có thể được đặt trong khoảng từ 0 đến 1.
    [Range(0f, 1f)]
    public float volume = 1f;

    // Cường độ âm thanh, có thể điều chỉnh tốc độ phát của âm thanh.
    [Range(.1f, 3f)]
    public float pitch = 1f;

    // Biến không được serialize, được sử dụng để lưu giữ giá trị mặc định của độ lớn khi âm thanh được khởi tạo.
    [NonSerialized] public float _defaultMaxVolume;

    // Cờ chỉ ra xem âm thanh có phải là lặp lại không.
    public bool isLoop;

    // Cờ chỉ ra xem âm thanh có cooldown không (phải chờ cho đến khi kết thúc trước khi có thể phát lại).
    public bool hasCooldown;

    // Cờ chỉ ra xem âm thanh có bỏ qua tình trạng tạm dừng của người nghe âm không.
    public bool ignoreListenerPause;

    // AudioSource được sử dụng để phát âm thanh. Được gán trong quá trình thiết lập.
    public AudioSource source;
}
