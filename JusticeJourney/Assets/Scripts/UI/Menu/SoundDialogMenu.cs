using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundDialogMenu : MonoBehaviour
{
    // Biến để lưu trữ tham chiếu đến thành phần UI
    [Header("Master Volume Setting")]
    [SerializeField] Slider _masterVolumeSlider;
    [SerializeField] TMP_Text _masterVolumeTextValue;
    [SerializeField] float _defaultMasterVolume = 50f;

    [Header("Effect Volume Setting")]
    [SerializeField] Slider _effectVolumeSlider;
    [SerializeField] TMP_Text _effectVolumeTextValue;
    [SerializeField] float _defaultEffectVolume = 100f;

    [Header("Music Volume Setting")]
    [SerializeField] Slider _musicVolumeSlider;
    [SerializeField] TMP_Text _musicVolumeTextValue;
    [SerializeField] float _defaultMusicVolume = 100f;

    [Header("Voice Volume Setting")]
    [SerializeField] Slider _voiceVolumeSlider;
    [SerializeField] TMP_Text _voiceVolumeTextValue;
    [SerializeField] float _defaultVoiceVolume = 100f;

    [Header("ConfirmationPopup")]
    [SerializeField] GameObject _confirmationPopup;

    [Header("BackButtonActionWithNoWarning")]
    [SerializeField] Button _backBtnActionWithNoWarning;

    // Biến để lưu trữ giá trị âm lượng mặc định và kiểm tra xem âm lượng có thay đổi hay không
    float _masterVolume;
    float _effectVolume;
    float _musicVolume;
    float _voiceVolume;
    bool _isAudioChanged;

    // Hằng số định danh cho PlayerPrefs
    const string MASTER_VOLUME = "masterVolume";
    const string EFFECT_VOLUME = "masterEffect";
    const string MUSIC_VOLUME = "masterMusic";
    const string VOICE_VOLUME = "masterVoice";

    // Phương thức được gọi khi đối tượng được kích hoạt
    void OnEnable()
    {
        // Đăng ký sự kiện để lắng nghe khi quá trình loadPrefs hoàn tất
        LoadPrefs.OnLoadingDone += LoadPrefs_LoadingDone;
    }
    // Phương thức được gọi khi đối tượng bị vô hiệu hóa
    void OnDisable()
    {
        // Hủy đăng ký sự kiện
        LoadPrefs.OnLoadingDone -= LoadPrefs_LoadingDone;
    }
    // Phương thức được gọi khi đối tượng được khởi tạo
    void Start()
    {
        // Khởi tạo giá trị âm lượng từ Slider và gán giá trị mặc định
        _masterVolume = _masterVolumeSlider.value;
        _effectVolume = _effectVolumeSlider.value;
        _musicVolume = _musicVolumeSlider.value;
        _voiceVolume = _voiceVolumeSlider.value;
        _isAudioChanged = false;
    }
    // Sự kiện khi quá trình LoadPrefs hoàn tất
    void LoadPrefs_LoadingDone()
    {
        // Reset trạng thái kiểm tra thay đổi âm thanh
        _isAudioChanged = false;
    }
    // Phương thức để thiết lập âm lượng chính
    public void SetMasterVolumes(float volume)
    {
        // Kiểm tra xem âm lượng có thay đổi hay không
        if (AudioListener.volume != volume * 0.01f)
            _isAudioChanged = true;
        // Cập nhật âm lượng và hiển thị trên UI
        AudioListener.volume = volume * 0.01f;
        _masterVolumeTextValue.text = volume.ToString("0");
        _masterVolume = volume;
    }
    
    public void SetEffectVolumes(float volume)
    {
        foreach (var sound in SoundManager.Instance.sounds)
        {
            if (sound.type == SoundManager.SoundTypes.Effect)
            {
                _isAudioChanged = true;
                sound.volume = sound._defaultMaxVolume * volume * 0.01f;
                sound.source.volume = sound._defaultMaxVolume * volume * 0.01f;
                _effectVolume = volume;
            }
        }
        _effectVolumeTextValue.text = volume.ToString();
    }

    public void SetMusicVolumes(float volume)
    {
        foreach (var sound in SoundManager.Instance.sounds)
        {
            if (sound.type == SoundManager.SoundTypes.Music)
            {
                _isAudioChanged = true;
                sound.volume = sound._defaultMaxVolume * volume * 0.01f;
                sound.source.volume = sound._defaultMaxVolume * volume * 0.01f;
                _musicVolume = volume;
            }
        }
        _musicVolumeTextValue.text = volume.ToString("0");
    }

    public void SetVoiceVolumes(float volume)
    {
        foreach (var sound in SoundManager.Instance.sounds)
        {
            if (sound.type == SoundManager.SoundTypes.Voice)
            {
                _isAudioChanged = true;
                sound.volume = sound._defaultMaxVolume * volume * 0.01f;
                sound.source.volume = sound._defaultMaxVolume * volume * 0.01f;
                _voiceVolume = volume;
            }
        }
        _voiceVolumeTextValue.text = volume.ToString("0");
    }
    // Phương thức để áp dụng các cài đặt âm thanh
    public void VolumeApply()
    {
        // Lưu giá trị âm lượng vào PlayerPrefs và reset trạng thái thay đổi âm thanh
        PlayerPrefs.SetFloat(MASTER_VOLUME, _masterVolume);
        PlayerPrefs.SetFloat(EFFECT_VOLUME, _effectVolume);
        PlayerPrefs.SetFloat(MUSIC_VOLUME, _musicVolume);
        PlayerPrefs.SetFloat(VOICE_VOLUME, _voiceVolume);
        _isAudioChanged = false;
    }
    // Phương thức để đặt lại tất cả các cài đặt âm thanh
    public void ResetButton()
    {
        // Gọi các phương thức đặt lại âm lượng chính và kiểm tra thay đổi âm thanh
        ResetMasterVolume();
        ResetEffectVolume();
        ResetMusicVolume();

        _isAudioChanged = true;
    }
    // Phương thức để đặt lại âm lượng chính
    public void ResetMasterVolume()
    {
        // Reset âm lượng chính về giá trị mặc định
        AudioListener.volume = _defaultMasterVolume * 0.01f;

        _masterVolume = _defaultMasterVolume;
        // Cập nhật giá trị trên Slider và hiển thị trên UI
        _masterVolumeSlider.value = _defaultMasterVolume;
        _masterVolumeTextValue.text = _defaultMasterVolume.ToString("0"); 
    }

    public void ResetEffectVolume()
    {
        foreach (var sound in SoundManager.Instance.sounds)
        {
            if (sound.type == SoundManager.SoundTypes.Effect)
            {
                sound.volume = sound._defaultMaxVolume;
                sound.source.volume = sound._defaultMaxVolume;
            }
        }
        _effectVolume = _defaultEffectVolume;

        _effectVolumeSlider.value = _defaultEffectVolume;
        _effectVolumeTextValue.text = _defaultEffectVolume.ToString("0");
    }

    public void ResetMusicVolume()
    {
        foreach (var sound in SoundManager.Instance.sounds)
        {
            if (sound.type == SoundManager.SoundTypes.Music)
            {
                sound.volume = sound._defaultMaxVolume;
                sound.source.volume = sound._defaultMaxVolume;
            }
        }
        _musicVolume = _defaultMusicVolume;

        _musicVolumeSlider.value = _defaultMusicVolume;
        _musicVolumeTextValue.text = _defaultMusicVolume.ToString("0");
    }

    public void ResetVoiceVolume()
    {
        foreach (var sound in SoundManager.Instance.sounds)
        {
            if (sound.type == SoundManager.SoundTypes.Voice)
            {
                sound.volume = sound._defaultMaxVolume;
                sound.source.volume = sound._defaultMaxVolume;
            }
        }
        _voiceVolume = _defaultVoiceVolume;

        _voiceVolumeSlider.value = _defaultVoiceVolume;
        _voiceVolumeTextValue.text = _defaultVoiceVolume.ToString("0");
    }
    // Phương thức kiểm tra và hiển thị cảnh báo khi thoát mà chưa lưu cài đặt âm thanh
    public void BackButton()
    {
        // Nếu có thay đổi âm thanh, hiển thị cảnh báo
        if (_isAudioChanged)
        {
            _confirmationPopup.SetActive(true);
        }
        else
        {
            // Ngược lại, thực hiện hành động khi nút Back được nhấn mà không cảnh báo
            _backBtnActionWithNoWarning.onClick.Invoke();
        }
    }
    // Phương thức để hủy bỏ các thay đổi âm thanh khi thoát cảnh báo
    public void DiscardAudioChanges()
    {
        _isAudioChanged = false;
    }

}
