using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // Một đối tượng ScoreManager được tạo ra để quản lý điểm số trong trò chơi
    public static ScoreManager Instance { get; private set; }

    // Biến lưu trữ số điểm của loại token
    public int tokenScore { get; private set; }

    // Biến lưu trữ số điểm của loại orb
    public int orbScore { get; private set; }

    // Biến lưu trữ số lượng quái vật đã tiêu diệt
    public int enemyKillCount { get; private set; }

    // Giao diện người dùng hiển thị số điểm của loại token
    [SerializeField] TMP_Text _tokenText;

    // Giao diện người dùng hiển thị số điểm của loại orb
    [SerializeField] TMP_Text _orbText;

    // Tên key để lưu trữ số điểm của loại token trong PlayerPrefs
    const string TOKEN_COUNT = "tokenCount";

    // Phương thức Awake được gọi khi đối tượng được tạo ra
    void Awake()
    {
        // Singleton pattern: Đảm bảo rằng chỉ có một đối tượng ScoreManager tồn tại trong trò chơi
        Instance = this;
    }

    // Phương thức Start được gọi khi trò chơi bắt đầu
    void Start()
    {
        // Khởi tạo giá trị ban đầu cho orbScore và hiển thị nó trên giao diện người dùng
        orbScore = 0;
        _orbText.SetText("x " + orbScore);

        // Load số điểm của loại token từ PlayerPrefs
        Load_Token();
    }

    // Phương thức được gọi khi một orb được thu thập
    public void Orb_Collected()
    {
        // Tăng số điểm của loại orb và cập nhật giao diện người dùng
        orbScore++;
        _orbText.SetText("x " + orbScore);
    }

    // Phương thức được gọi khi một token được kiếm được
    public void Token_Earned(int earnedAmount)
    {
        // Tăng số điểm của loại token và cập nhật giao diện người dùng
        tokenScore += earnedAmount;
        _tokenText.SetText("x " + tokenScore);

        // Lưu số điểm của loại token vào PlayerPrefs
        PlayerPrefs.SetInt(TOKEN_COUNT, tokenScore);
    }

    // Phương thức được gọi khi một token được tiêu thụ
    public void Token_Spent(int spentAmount)
    {
        // Giảm số điểm của loại token và cập nhật giao diện người dùng
        tokenScore -= spentAmount;
        _tokenText.SetText("x " + tokenScore);

        // Lưu số điểm của loại token vào PlayerPrefs
        PlayerPrefs.SetInt(TOKEN_COUNT, tokenScore);
    }

    // Phương thức được gọi để load số điểm của loại token từ PlayerPrefs
    void Load_Token()
    {
        if (PlayerPrefs.HasKey(TOKEN_COUNT))
            tokenScore = PlayerPrefs.GetInt(TOKEN_COUNT);
        else
            tokenScore = 0;

        // Cập nhật giao diện người dùng với số điểm của loại token
        _tokenText.SetText("x " + tokenScore);
    }

    // Phương thức được gọi khi một quái vật được tiêu diệt
    public void Enemy_Killed()
    {
        // Tăng số lượng quái vật đã tiêu diệt
        enemyKillCount++;
    }
}
