using UnityEngine;
using TMPro;

public class MerchantTalk1 : MonoBehaviour
{
    [SerializeField] UI_Assistant _uiAssistant;
    [SerializeField] TMP_Text _talkText;
    [SerializeField] UI_Shop _uiShop;

    IShopCustomer _shopCustomer;

    string[] _initialDialog;
    SoundManager.SoundTags[] _dialogSounds;
    float[] _typeSpeed;

    bool _isPlayerInRange;
    static bool _hasTalkedOnce;

    void OnEnable()
    {
        Player.Instance.inputHandler.OnTalkAction += PlayerTalkPressed;
        _uiAssistant.OnOpenShop += UIAssistant_OpenShop;
    }

    void OnDisable()
    {
        Player.Instance.inputHandler.OnTalkAction -= PlayerTalkPressed;
        _uiAssistant.OnOpenShop -= UIAssistant_OpenShop;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IShopCustomer>(out _shopCustomer))
        {
            _talkText.gameObject.SetActive(true);
            _talkText.text = "Shop (" + GameAssets.Instance.keybinds[(int)GameAssets.Keybinds.Interact].text + ")";
            _isPlayerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IShopCustomer>(out _shopCustomer))
        {
            _talkText.text = "";
            _isPlayerInRange = false;
            _talkText.gameObject.SetActive(false);
            _uiAssistant.gameObject.SetActive(false);
            _uiShop.Hide();
            _uiAssistant.StopTalkingSound();
        }
    }

    void PlayerTalkPressed()
    {
        if (!_isPlayerInRange || GameManager.Instance.currentState == PlayPauseState.Paused)
            return;

        if (_hasTalkedOnce)
        {
            _uiShop.Show(_shopCustomer);
            return;
        }

        _initialDialog = new string[]
        {
          "Ồ, chào! Không giống như anh chàng áo xanh đằng kia, tôi không biết nhiều ngôn ngữ của bạn. Tôi thậm chí còn không biết làm thế nào mà anh ấy có được giọng nói thực sự.",
  "Tôi bán kiếm, trang phục và kích thích giúp nâng cao tính cách của bạn.",
  "Tuy nhiên, hãy cẩn thận, công việc kinh doanh của tôi chỉ kết thúc với những viên đá quý bạn thu thập được từ mỏ!",
        };

        _typeSpeed = new float[]
        {
            0.05f,
            0.05f,
            0.05f,
        };

        _uiAssistant.CloseDarknessTalk();
        _uiAssistant.gameObject.SetActive(true);
        _uiAssistant.NpcTalk(_initialDialog, _typeSpeed);

    }

    void UIAssistant_OpenShop()
    {
        _hasTalkedOnce = true;
        _uiShop.Show(_shopCustomer);
    }

}
