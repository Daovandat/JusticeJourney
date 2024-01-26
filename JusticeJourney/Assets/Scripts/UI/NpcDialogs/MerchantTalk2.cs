using UnityEngine;
using TMPro;

public class MerchantTalk2 : MonoBehaviour
{
    [SerializeField] UI_Assistant _uiAssistant;
    [SerializeField] TMP_Text _talkText;
    [SerializeField] UI_Shop _uiShop;

    IShopCustomer _shopCustomer;

    string[] _initialDialog;
    SoundManager.SoundTags[] _dialogSounds;
    float[] _typeSpeed;

    bool _isPlayerInRange;
    static bool _HasTalkedOnce;

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

        if (_HasTalkedOnce)
        {
            _uiShop.Show(_shopCustomer);
            return;
        }

        _initialDialog = new string[]
        {
            "Xin chào lần nữa, tôi nghĩ tốt nhất chúng ta nên tiếp tục thỏa thuận ở khu mỏ này.",
"Hãy thoải mái lấy bất cứ thứ gì miễn là bạn có đá quý.",
        };

        _dialogSounds = new SoundManager.SoundTags[]
        {
        SoundManager.SoundTags.MerchantTalk,
        SoundManager.SoundTags.MerchantTalk,
        };

        _typeSpeed = new float[]
        {
            0.05f,
            0.05f,
        };

        _uiAssistant.gameObject.SetActive(true);
        _uiAssistant.NpcTalk(_initialDialog, _typeSpeed);

    }

    void UIAssistant_OpenShop()
    {
        _HasTalkedOnce = true;
        _uiShop.Show(_shopCustomer);
    }

}
