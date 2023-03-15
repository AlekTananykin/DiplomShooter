using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMessagePanel : MonoBehaviour
{
    [SerializeField]
    private Button _ok_Button;
    [SerializeField]
    private TMP_Text _message_Text;

    private void Start()
    {
        this.gameObject.SetActive(false);
        _ok_Button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        this.gameObject.SetActive(false);
    }

    public void ViewMessage(string message)
    {
        this.gameObject.SetActive(true);
        _message_Text.text = message;
    }

}
