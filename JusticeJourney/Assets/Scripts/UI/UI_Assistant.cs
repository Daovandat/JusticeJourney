using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Assistant : MonoBehaviour
{
    public Action OnSpeechEnd;
    public Action OnOpenShop;

    [SerializeField] TMP_Text _messageText;
    [SerializeField] GameObject _darknessTalk;

    Button _button;
    TextWriter.TextWriterSingle _textWriterSingle;
    static SoundManager.SoundTags _PreviousDialogSound;

    void Awake()
    {
        _button = transform.Find("message").GetComponent<Button>();
    }

    public void NpcTalk(string[] messages, float[] typeSpeed)
    {
        _textWriterSingle = TextWriter.AddWriter_Static(_messageText, messages[0], typeSpeed[0], true, true, StopTalkingSound);

        int count = 1;

        _button.onClick.RemoveAllListeners();

        _button.onClick.AddListener(() =>
        {
            if (GameManager.Instance.currentState == PlayPauseState.Paused)
                return;

            if (_textWriterSingle != null && _textWriterSingle.IsActive())
            {
                // Currently active TextWriter
                _textWriterSingle.WriteAllAndDestroy();
            }
            else
            {
                if (count >= messages.Length)
                {
                    OnOpenShop?.Invoke();
                    gameObject.SetActive(false);
                    return;
                }

                _textWriterSingle = TextWriter.AddWriter_Static(_messageText, messages[count], typeSpeed[count], true, true, StopTalkingSound);
                count++;
            }
        });
    }

    public void StopTalkingSound()
    {
        if (_textWriterSingle != null && _textWriterSingle.IsActive())
        {
            // Currently active TextWriter
            _textWriterSingle.WriteAllAndDestroy();
        };
        OnSpeechEnd?.Invoke();
    }

    public void CloseDarknessTalk()
    {
        if (_textWriterSingle != null && _textWriterSingle.IsActive())
        {
            // Currently active TextWriter
            _textWriterSingle.WriteAllAndDestroy();
        }

        SoundManager.Instance.Stop(_PreviousDialogSound);
        _darknessTalk.SetActive(false);
    }

}
