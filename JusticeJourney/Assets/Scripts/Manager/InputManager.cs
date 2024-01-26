using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // Singleton pattern: Biến Instance để truy cập từ bất kỳ đâu trong code
    public static InputManager Instance { get; private set; }

    // Sự kiện được kích hoạt khi nút Pause được nhấn
    public Action OnPauseAction;

    // Tham chiếu đến PlayerInput trong scene
    [SerializeField] PlayerInput _playerInput;

    // InputAction để theo dõi nút Pause
    InputAction _pauseAction;

    // Phương thức Awake được gọi khi đối tượng được khởi tạo
    void Awake()
    {
        // Singleton pattern: Đảm bảo chỉ có một đối tượng InputManager tồn tại
        Instance = this;

        // Lấy tham chiếu đến InputAction "Pause" từ PlayerInput
        _pauseAction = _playerInput.actions["Pause"];
    }

    // Phương thức OnEnable được gọi khi đối tượng được bật hoạt động
    void OnEnable()
    {
        // Gắn kết sự kiện khi nút Pause được nhấn
        _pauseAction.started += PauseStart;
    }

    // Phương thức OnDisable được gọi khi đối tượng bị tắt hoạt động
    void OnDisable()
    {
        // Hủy kết sự kiện khi nút Pause được nhấn
        _pauseAction.started -= PauseStart;
    }

    // Phương thức được gọi khi nút Pause được nhấn
    void PauseStart(InputAction.CallbackContext context)
    {
        // Kích hoạt sự kiện OnPauseAction (nếu có)
        OnPauseAction?.Invoke();
    }
}
