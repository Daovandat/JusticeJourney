using UnityEngine;
using TMPro;

public class NpcTalk1 : MonoBehaviour
{
    [SerializeField] UI_Assistant _uiAssistant;
    [SerializeField] TMP_Text _talkText;

    string[] _initialDialog;
  //  SoundManager.SoundTags[] _dialogSounds;
    float[] _typeSpeed;

    bool _isPlayerInRange;

    void OnEnable()
    {
        Player.Instance.inputHandler.OnTalkAction += PlayerTalkPressed;
    }

    void OnDisable()
    {
        Player.Instance.inputHandler.OnTalkAction -= PlayerTalkPressed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        _talkText.gameObject.SetActive(true);
        _talkText.text = "Talk (" + GameAssets.Instance.keybinds[(int)GameAssets.Keybinds.Interact].text + ")";
        _isPlayerInRange = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        _talkText.text = "";
        _isPlayerInRange = false;
        _talkText.gameObject.SetActive(false);
        _uiAssistant.gameObject.SetActive(false);
        _uiAssistant.StopTalkingSound();
    }

    void PlayerTalkPressed()
    {
        if (!_isPlayerInRange || GameManager.Instance.currentState == PlayPauseState.Paused)
            return;

        _initialDialog = new string[]
        {
           "Chào mừng người lạ, ý tôi là anh hùng!. Chúng tôi đang đợi bạn!",
 "Các mỏ khai thác của chúng tôi đang bị tấn công. Các loài vĩnh cửu gần như đã kiểm soát hoàn toàn chúng.",
 "Chúng tôi đã bắt được một số loài trong số đó và chúng tôi muốn huấn luyện bạn chống lại chúng.",
 "Hãy tiếp tục và bắt đầu quá trình huấn luyện của bạn. Ồ và làm ơn đừng giết những con chim, chúng thật dễ thương. Tôi yêu chúng.",
        };

      //  _dialogSounds = new SoundManager.SoundTags[]
      //  {
      //  SoundManager.SoundTags.NpcTalk1_1,
      //  SoundManager.SoundTags.NpcTalk1_2,
       // SoundManager.SoundTags.NpcTalk1_3,
       // SoundManager.SoundTags.NpcTalk1_4,
      //  };

        _typeSpeed = new float[]
        {
            0.07f,
            0.062f,
            0.05f,
            0.06f
        };

        _uiAssistant.CloseDarknessTalk();
        _uiAssistant.gameObject.SetActive(true);
        _uiAssistant.NpcTalk(_initialDialog, _typeSpeed );
        
    }

}
