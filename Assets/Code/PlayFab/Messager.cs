using System;
using TMPro;
using UnityEngine;

public class Messager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    [SerializeField]
    private GameObject _button;

    public Action OnButtonClick;

    public void ButtonClick()
    {
        _text.SetText("");
        OnButtonClick?.Invoke();
    }

    public void ViewMessage(string message)
    {
        _text.SetText(message);
    }
}
