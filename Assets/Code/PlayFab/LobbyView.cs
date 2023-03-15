
using PlayFab;
using PlayFab.ClientModels;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyView : MonoBehaviour
{
    public Action OnExitButtonClick;

    public void ExitButtonClick()
    {
        _text.SetText("Exit");
        OnExitButtonClick?.Invoke();
    }

    public void ViewMessage()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(),
        OnGetAccountSuccess, OnFailure);
    }

    private void OnGetAccountSuccess(GetAccountInfoResult result)
    {
        _text.text = $"Welcome back, Player ID {result.AccountInfo.PlayFabId}";

        _catalogReader?.Read();
    }
    private void OnFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        _text.text = errorMessage;
        Debug.LogError($"Something went wrong: {errorMessage}");
    }

    private void Start()
    {
        _selectCharButton.onClick.AddListener(OnSelectCharButtonClick);
        _characterManager.SetActive(false);
    }

    public void OnSelectCharButtonClick()
    {
        _characterManager.SetActive(true);
        gameObject.SetActive(false);
    }

    [SerializeField]
    TextMeshProUGUI _text;

    [SerializeField]
    Button _selectCharButton;

    [SerializeField]
    CatalogReader _catalogReader;
    [SerializeField]
    private GameObject _characterManager;
}
