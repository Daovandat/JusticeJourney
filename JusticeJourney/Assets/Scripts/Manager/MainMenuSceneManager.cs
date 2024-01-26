using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneManager : MonoBehaviour
{
    // Phương thức Start được gọi khi đối tượng được khởi tạo
    void Start()
    {
        // Thiết lập current scene là MainMenu và chơi âm thanh nền
        ApplicationModel.CurrentScene = (int)Loader.Scene.MainMenu;
        SoundManager.Instance.Play(SoundManager.SoundTags.Ambiance1);
    }

    // Phương thức được gọi khi nút "Play Game" được nhấn
    public void PlayGame()
    {
        // Lưu trạng thái của current scene làm previous scene, chuyển sang Loading scene để tải scene mới
        ApplicationModel.PreviousScene = ApplicationModel.CurrentScene;
        ApplicationModel.CurrentScene = (int)Loader.Scene.Scene1;
        SceneManager.LoadScene(Loader.Scene.Loading.ToString());
    }

    // Phương thức được gọi khi nút "Exit Game" được nhấn
    public void ExitGame()
    {
        // Thoát ứng dụng
        Application.Quit();
    }
}
