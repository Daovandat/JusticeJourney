using UnityEngine;
using Cinemachine;

// Enum định nghĩa trạng thái game có thể ở trong trạng thái đang chơi hoặc đã tạm dừng
public enum PlayPauseState
{
    Playing,
    Paused,
}

public class GameManager : MonoBehaviour
{
    // Singleton pattern để có thể truy cập từ bất kỳ đâu trong game
    public static GameManager Instance { get; private set; }

    // Trạng thái hiện tại của game
    public PlayPauseState currentState { get; private set; }

    // Các đối tượng trong game được tham chiếu từ trình biên dịch
    [SerializeField] GameObject _menu;
    [SerializeField] GameObject[] _panelsToClose;
    [SerializeField] GameObject _gameoverPanel;

    [SerializeField] Transform _spawnPoint;
    [SerializeField] Transform _returnedSpawnPoint;

    CinemachineVirtualCamera _cvc;

    // Hàm được gọi khi đối tượng được tạo
    void Awake()
    {
        // Singleton pattern
        Instance = this;

        // Lấy tham chiếu đến CinemachineVirtualCamera
        _cvc = transform.parent.Find("Cameras").Find("Player Camera").GetComponent<CinemachineVirtualCamera>();

        // Trạng thái mặc định khi bắt đầu game là đang chơi
        currentState = PlayPauseState.Playing;
    }

    // Hàm được gọi khi đối tượng được kích hoạt
    void OnEnable()
    {
        // Thiết lập lại thời gian thực hiện trong game và đăng ký sự kiện khi người chơi chết
        Time.timeScale = 1f;
        Player.Instance.OnPlayerDied += Player_PlayerDied;
    }

    // Hàm được gọi khi đối tượng không còn kích hoạt
    void OnDisable()
    {
        // Hủy đăng ký sự kiện khi người chơi chết và sự kiện khi người chơi tạm dừng
        Player.Instance.OnPlayerDied += Player_PlayerDied;
        InputManager.Instance.OnPauseAction -= InputManager_PausePressed;
    }

    // Hàm được gọi khi đối tượng được tạo
    void Start()
    {
        // Đăng ký sự kiện khi nút tạm dừng được nhấn
        InputManager.Instance.OnPauseAction += InputManager_PausePressed;

        // Phát âm nhạc nền tùy thuộc vào scene hiện tại
        PlayAmbianceMusicAccordingToScene();

        // Thiết lập điểm xuất hiện của người chơi tùy thuộc vào scene
        PlayerSpawnPointAccordingToScene();

        // Ngăn chặn một bug kỳ lạ trên WebGL
        AudioListener.pause = true;
        Time.timeScale = 0f;

        // Mở âm thanh và thời gian thực hiện trong game
        AudioListener.pause = false;
        Time.timeScale = 1f;
    }

    // Hàm xử lý khi nút tạm dừng được nhấn
    void InputManager_PausePressed()
    {
        // Nếu panel Game Over đang hiển thị thì không làm gì
        if (_gameoverPanel.activeSelf)
            return;

        // Kiểm tra xem có bất kỳ panel nào đang mở hay không, nếu có thì đóng nó
        bool isPanelOn = false;
        foreach (GameObject panel in _panelsToClose)
        {
            if (panel.activeSelf)
            {
                panel.SetActive(false);
                isPanelOn = true;
                SoundManager.Instance.Play(SoundManager.SoundTags.ButtonClick);
            }
        }

        // Nếu không có panel nào mở và trạng thái hiện tại là đang chơi, tạm dừng game
        if (!isPanelOn && currentState == PlayPauseState.Playing)
            Game_Paused();
    }

    // Hàm xử lý khi người chơi chết
    void Player_PlayerDied()
    {
        // Ẩn đối tượng người chơi, thiết lập vị trí xuất hiện mới, và hiển thị panel Game Over
        Player.Instance.gameObject.SetActive(false);
        Player.Instance.transform.position = _spawnPoint.position;
        _cvc.m_Follow = null;
        _gameoverPanel.SetActive(true);

        // Dừng âm thanh khi người chơi chạy
        SoundManager.Instance.Stop(SoundManager.SoundTags.PlayerRun);

        // Dừng âm nhạc nền tùy thuộc vào scene
        StopAmbianceMusicAccordingToScene();

        // Phát âm thanh Game Over
        SoundManager.Instance.Play(SoundManager.SoundTags.Gameover);
    }

    // Phương thức để hồi sinh người chơi
    public void Respawn()
    {
        // Theo dõi người chơi, hiển thị người chơi và chơi lại âm nhạc nền
        _cvc.m_Follow = Player.Instance.transform;
        Player.Instance.gameObject.SetActive(true);
        SoundManager.Instance.Stop(SoundManager.SoundTags.Gameover);
        PlayAmbianceMusicAccordingToScene();
    }

    // Phương thức để rời khỏi scene 2 (được gọi từ button "Leave Scene 2")
    public void LeaveScene2()
    {
        // Tạm dừng game, hiển thị panel Game Over, và chơi âm thanh Game Over
        Time.timeScale = 0f;
        _gameoverPanel.SetActive(true);
        currentState = PlayPauseState.Paused;

        // Dừng âm nhạc nền tùy thuộc vào scene
        StopAmbianceMusicAccordingToScene();
        SoundManager.Instance.Play(SoundManager.SoundTags.Gameover);
    }

    // Phương thức được gọi khi game được tiếp tục sau khi tạm dừng
    public void Game_Resumed()
    {
        // Mở âm thanh và thời gian thực hiện trong game, đặt trạng thái là đang chơi
        AudioListener.pause = false;
        Time.timeScale = 1f;
        currentState = PlayPauseState.Playing;
    }

    // Phương thức được gọi khi game tạm dừng
    public void Game_Paused()
    {
        // Nếu game đang không chơi, không làm gì
        if (currentState != PlayPauseState.Playing)
            return;

        // Dừng âm thanh, phát âm thanh khi nút tạm dừng được nhấn, và hiển thị menu tạm dừng
        AudioListener.pause = true;
        SoundManager.Instance.Play(SoundManager.SoundTags.ButtonClick);
        _menu.gameObject.SetActive(true);
        Time.timeScale = 0f;
        currentState = PlayPauseState.Paused;
    }

    // Phương thức phát âm nhạc nền tùy thuộc vào scene
    void PlayAmbianceMusicAccordingToScene()
    {
        if (ApplicationModel.CurrentScene == (int)Loader.Scene.Scene1)
            SoundManager.Instance.Play(SoundManager.SoundTags.Ambiance2);
        else
            SoundManager.Instance.Play(SoundManager.SoundTags.Ambiance3);
    }

    // Phương thức dừng âm nhạc nền tùy thuộc vào scene
    void StopAmbianceMusicAccordingToScene()
    {
        if (ApplicationModel.CurrentScene == (int)Loader.Scene.Scene1)
            SoundManager.Instance.Stop(SoundManager.SoundTags.Ambiance2);
        else
            SoundManager.Instance.Stop(SoundManager.SoundTags.Ambiance3);
    }

    // Phương thức đặt vị trí xuất hiện của người chơi tùy thuộc vào scene
    void PlayerSpawnPointAccordingToScene()
    {
        if (ApplicationModel.PreviousScene == (int)Loader.Scene.Scene1)
        {
            Player.Instance.transform.position = _spawnPoint.position;
        }
        else if (ApplicationModel.PreviousScene == (int)Loader.Scene.Scene2)
        {
            Player.Instance.transform.position = _returnedSpawnPoint.position;
        }
    }
}
