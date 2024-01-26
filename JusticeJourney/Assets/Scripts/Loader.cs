using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Loader : MonoBehaviour
{
    [SerializeField] Slider _loadingSlider;     // Thanh trạng thái tải
    [SerializeField] TMP_Text progressText;    // Văn bản tiến độ

    public enum Scene
    {
        MainMenu,   // Màn hình chính
        Loading,    // Màn hình tải
        Scene1,     // Scene 1
        Scene2,     // Scene 2
    }

    // Coroutine để xử lý tải scene
    IEnumerator Start()
    {
        // Đợi 0.1 giây (chỉ là để tạo hiệu ứng vui nhộn)
        yield return new WaitForSeconds(0.1f);

        AsyncOperation operation;

        // Kiểm tra scene hiện tại và tải scene tương ứng
        if (ApplicationModel.CurrentScene == (int)Scene.Scene1)
            operation = SceneManager.LoadSceneAsync(Scene.Scene1.ToString());
        else
            operation = SceneManager.LoadSceneAsync(Scene.Scene2.ToString());

        // Lặp đến khi quá trình tải hoàn tất
        while (operation.isDone == false)
        {
            // Chuẩn hóa giá trị tiến độ trong khoảng 0 đến 1
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            // Làm tròn giá trị tiến độ đến 2 chữ số thập phân
            progress = (float)System.Math.Round(progress, 2);

            // Cập nhật giá trị trên thanh trạng thái tải và văn bản tiến độ
            _loadingSlider.value = progress;
            progressText.text = (progress * 100f).ToString("F0") + "%";

            // Chờ một frame kế tiếp
            yield return null;
        }
    }
}
