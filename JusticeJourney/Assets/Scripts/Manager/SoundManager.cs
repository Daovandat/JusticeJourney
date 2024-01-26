using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Mẫu thiết kế Singleton: instance tĩnh của SoundManager
    public static SoundManager Instance { get; private set; }

    // Mảng chứa thông tin về các âm thanh khác nhau
    public Sound[] sounds;
    // Từ điển để lưu trữ thời điểm cuối cùng mỗi âm thanh được phát lại
    private static Dictionary<SoundTags, float> soundTimerDictionary;

    // Enum để biểu diễn các loại âm thanh khác nhau
    public enum SoundTags
    {
        GrassMove1,
        GrassMove2,
        GrassMove3,
        PlayerMelee1,
        PlayerMelee2,
        PlayerMelee3,
        PlayerDash,
        PlayerDashPre,
        PlayerHurt1,
        PlayerHurt2,
        PlayerJump,
        PlayerLand,
        PlayerParry,
        PlayerRun,
        PlayerWallSlide,
        SkeletonAttack1,
        SkeletonAttack2,
        SkeletonAttack3,
        SkeletonBow,
        SkeletonDetection1,
        SkeletonDetection2,
        SkeletonDie,
        SkeletonDodge,
        SkeletonHurt,
        SkeletonRespawn,
        SkeletonSpell,
        SkeletonTeleport,
        PlayerLedgeClimb,
        NpcTalk1_1,
        NpcTalk1_2,
        NpcTalk1_3,
        NpcTalk1_4,
        NpcTalk2_1,
        NpcTalk3_1,
        NpcTalk3_2,
        NpcTalk4_1,
        NpcTalk5_1,
        NpcTalk5_2,
        NpcTalk5_3,
        NpcTalk6_1,
        NpcTalk6_2,
        NpcTalk6_3,
        NpcTalk6_4,
        NpcTalk7_1,
        NpcTalk7_2,
        ButtonClick,
        DarknessTalk1,
        DarknessTalk2,
        DarknessTalk3,
        DarknessTalk4,
        DarknessTalkOnDeath,
        ScoreCalculation,
        Powerup,
        DropLoot1,
        DropLoot2,
        DropLoot3,
        Shield,
        MerchantTalk,
        ShopSword,
        ShopOutfit,
        ShopBoosts,
        Ambiance1,
        Ambiance2,
        Ambiance3,
        Gameover,
        NpcTalk1_5
    }

    // Enum để phân loại âm thanh thành các loại khác nhau
    public enum SoundTypes
    {
        Effect,
        Music,
        Voice,
    }

    // Awake được gọi khi thể hiện của script được nạp
    void Awake()
    {
        // Thiết lập instance Singleton
        Instance = this;

        // Khởi tạo từ điển thời gian cho âm thanh
        soundTimerDictionary = new Dictionary<SoundTags, float>();

        // Lặp qua từng âm thanh trong mảng sounds
        foreach (Sound sound in sounds)
        {
            // Thêm một thành phần AudioSource vào GameObject
            sound.source = gameObject.AddComponent<AudioSource>();
            // Thiết lập các thuộc tính của AudioSource dựa trên đối tượng Sound
            sound.source.clip = sound.clip;
            sound._defaultMaxVolume = sound.volume;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.isLoop;
            sound.source.ignoreListenerPause = sound.ignoreListenerPause;
            sound.source.playOnAwake = false;

            // Nếu âm thanh có thời gian cooldown, thiết lập một bộ hẹn giờ ban đầu
            if (sound.hasCooldown)
            {
                soundTimerDictionary[sound.name] = 0.15f;
            }
        }
    }

    // Start được gọi trước khi khung hình đầu tiên được hiển thị
    void Start()
    {
        // Đã comment, nhưng có thể được sử dụng để phát một âm thanh khởi đầu khi script bắt đầu
        //Play(SoundManager.SoundTags.Ambient);
    }
    // Phương thức để phát một âm thanh cụ thể
    public void Play(SoundTags name)
    {
        // Tìm đối tượng Sound với tên cụ thể trong mảng sounds
        Sound sound = Array.Find(sounds, s => s.name == name);
        // Nếu không tìm thấy âm thanh, log lỗi và trả về
        if (sound == null)
        {
            Debug.LogError("Sound " + name + " Not Found!");
            return;
        }
        // Nếu âm thanh có thể được phát dựa trên cooldown, phát âm thanh
        if (!CanPlaySound(sound)) return;

        sound.source.Play();
    }
    // Phương thức để dừng một âm thanh cụ thể nếu nó đang phát
    public void Stop(SoundTags name)
    {
        // Tìm đối tượng Sound với tên cụ thể trong mảng sound
        Sound sound = Array.Find(sounds, s => s.name == name);

        // Nếu không tìm thấy âm thanh, log lỗi và trả v
        if (sound == null)
        {
            Debug.LogError("Sound " + name + " Not Found!");
            return;
        }

        // Nếu âm thanh đang phát, dừng nó
        if (sound.source.isPlaying)
            sound.source.Stop();
    }

    // Phương thức để tạm dừng một âm thanh cụ thể nếu nó đang phát
    public void Puase(SoundTags name)
    {
        // Tìm đối tượng Sound với tên cụ thể trong mảng sounds
        Sound sound = Array.Find(sounds, s => s.name == name);

        // Nếu không tìm thấy âm thanh, log lỗi và trả về
        if (sound == null)
        {
            Debug.LogError("Sound " + name + " Not Found!");
            return;
        }

        // Nếu âm thanh đang phát, tạm dừng nó
        if (sound.source.isPlaying)
            sound.source.Pause();
    }

    // Kiểm tra xem âm thanh có thể được phát dựa trên hệ thống cooldown khôn
    private static bool CanPlaySound(Sound sound)
    {
        // Nếu âm thanh có trong từ điển hẹn giờ
        if (soundTimerDictionary.ContainsKey(sound.name))
        {
            // Lấy thời điểm cuối cùng âm thanh được phát
            float lastTimePlayed = soundTimerDictionary[sound.name];
            // Nếu đã đủ thời gian trôi qua kể từ lần phát cuối cùng, cập nhật bộ hẹn giờ và trả về true
            if (lastTimePlayed + (sound.clip.length) / 8 < Time.unscaledTime)
            {
                soundTimerDictionary[sound.name] = Time.unscaledTime;
                return true;
            }
            // Nếu chưa đủ thời gian trôi qua, trả về false
            return false;
        }
        // Nếu âm thanh không có trong từ điển hẹn giờ, trả về true
        return true;
    }

}