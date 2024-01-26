using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    // Singleton pattern: một thể hiện (instance) duy nhất của GameSceneManager có thể truy cập từ bất kỳ đâu.
    public static GameSceneManager Instance { get; private set; }

    void Awake()
    {
        // Thiết lập Instance thành thể hiện hiện tại của lớp GameSceneManager khi nó được khởi tạo.
        Instance = this;
    }

    // Phương thức để quay lại màn hình chính.
    public void ReturnToMainMenu()
    {
        // Đặt tỉ lệ thời gian về mức bình thường.
        Time.timeScale = 1f;

        // Load màn hình chính bằng cách sử dụng SceneManager và tên màn hình từ enum Loader.Scene.
        SceneManager.LoadScene(Loader.Scene.MainMenu.ToString());
    }

    // Phương thức để tải Scene1.
    public void LoadScene1()
    {
        // Đặt tỉ lệ thời gian về mức bình thường.
        Time.timeScale = 1f;

        // Lưu chỉ số của scene hiện tại làm scene trước đó.
        ApplicationModel.PreviousScene = ApplicationModel.CurrentScene;

        // Đặt chỉ số của scene hiện tại thành Scene1 và load một màn hình loading.
        ApplicationModel.CurrentScene = (int)Loader.Scene.Scene1;
        SceneManager.LoadScene(Loader.Scene.Loading.ToString());
    }

    // Phương thức để tải Scene2.
    public void LoadScene2()
    {
        // Đặt tỉ lệ thời gian về mức bình thường.
        Time.timeScale = 1f;

        // Lưu chỉ số của scene hiện tại làm scene trước đó.
        ApplicationModel.PreviousScene = ApplicationModel.CurrentScene;

        // Đặt chỉ số của scene hiện tại thành Scene2 và load một màn hình loading.
        ApplicationModel.CurrentScene = (int)Loader.Scene.Scene2;
        SceneManager.LoadScene(Loader.Scene.Loading.ToString());
    }
}
