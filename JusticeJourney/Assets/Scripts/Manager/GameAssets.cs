using UnityEngine;
using TMPro;

public class GameAssets : MonoBehaviour
{
    // Singleton pattern để có thể truy cập từ bất kỳ đâu trong game
    public static GameAssets Instance { get; private set; }

    void Awake()
    {
        // Singleton pattern
        Instance = this;
    }

    // Enum định nghĩa các loại phím tắt trong game
    public enum Keybinds
    {
        MoveLeft,
        MoveRight,
        Jump,
        Attack,
        Parry,
        Dash,
        Crouch,
        Interact
    }

    // Mảng chứa các đối tượng TMP_Text để hiển thị keybinds
    [SerializeField] TMP_Text[] _keybinds;
    public TMP_Text[] keybinds { get { return _keybinds; } private set { keybinds = value; } }

    // Các sprite cho các loại kiếm khác nhau
    [Header("Sprite Swords")]
    public Sprite[] s_DefaultSword;
    public Sprite[] s_BlueSword;
    public Sprite[] s_CyanSword;
    public Sprite[] s_GreenSword;
    public Sprite[] s_RedSword;
    public Sprite[] s_PurpleSword;

    // Các sprite cho các loại trang phục khác nhau
    [Header("Sprite Outfits")]
    public Sprite[] s_DefaultOutfit;
    public Sprite[] s_BlueOutfit;
    public Sprite[] s_GreenOutfit;
    public Sprite[] s_YellowOutfit;
    public Sprite[] s_BrownOutfit;

    // Các sprite cho các loại boost khác nhau
    [Header("Sprite Boosts")]
    public Sprite[] s_DefenseBoost;
    public Sprite[] s_OffenseBoost;

    // Mảng màu sắc cho các loại SwordTrailEffects khác nhau
    [Header("Color SwordTrailEffects")]
    public Color[] c_DefaultTrail;
    public Color[] c_BlueTrail;
    public Color[] c_CyanTrail;
    public Color[] c_GreenTrail;
    public Color[] c_RedTrail;
    public Color[] c_PurpleTrail;
}
