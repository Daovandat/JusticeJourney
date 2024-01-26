using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NpcTalk6 : MonoBehaviour
{
    [SerializeField] UI_Assistant _uiAssistant;
    [SerializeField] TMP_Text _talkText;

    string[] _initialDialog;
    SoundManager.SoundTags[] _dialogSounds;
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
        _talkText.text = "Listen (" + GameAssets.Instance.keybinds[(int)GameAssets.Keybinds.Interact].text + ")";
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
            "Ah, sự vĩnh cửu.. Hãy để bạn chiến đấu ngay bây giờ.",
            "Nhấn ["+ GameAssets.Instance.keybinds[(int)GameAssets.Keybinds.Attack].text +"] tấn công, " +
            "[" + GameAssets.Instance.keybinds[(int)GameAssets.Keybinds.Parry].text + "] để đỡ đòn tấn công sắp tới.",
            "Nếu bạn căn thời gian hợp lý, bạn thực hiện Cú đỡ hoàn hảo mà không nhận đòn nào, đồng thời làm choáng kẻ thù.",
            "Tất cả những gì bạn cần làm là đỡ đòn ngay trước khi cuộc tấn công sắp tới, và chỉ vậy thôi!",
        };

        _dialogSounds = new SoundManager.SoundTags[]
        {
        SoundManager.SoundTags.NpcTalk6_1,
        SoundManager.SoundTags.NpcTalk6_2,
        SoundManager.SoundTags.NpcTalk6_3,
        SoundManager.SoundTags.NpcTalk6_4,
        };

        _typeSpeed = new float[]
        {
            0.062f,
            0.06f,
            0.06f,
            0.055f,
        };

        _uiAssistant.CloseDarknessTalk();
        _uiAssistant.gameObject.SetActive(true);
        _uiAssistant.NpcTalk(_initialDialog, _typeSpeed);

    }

}
