using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabSignIn : MonoBehaviour
{
    public Action<string> OnRegistrationSuccess;
    public Action<string> OnRegistrationError;

    public Action<string> OnSignInSuccess;
    public Action<string> OnSignInError;

    public void CreateAccount(string email, string username, string password) 
    {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest() 
        {
            Username = username,
            Password = password,
            Email = email,
            RequireBothUsernameAndEmail = true
        }, 
        result => { OnRegistrationSuccess(result.ToString()); }, 
        error => { OnRegistrationError(error.ToString()); });
    }

    public void SignIn(string username, string password)
    {
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest() 
        {
            Username = username,
            Password = password
        },
        result => { OnSignInSuccess(result.ToString()); },
        error => { OnSignInError(error.ToString()); });
    }


    public string Mail { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }


    public void Connect()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "4FEF2";
        }
        var request = new LoginWithCustomIDRequest
        {
            CustomId = "Lesson3",
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(
            request,
            OnLoginSuccess,
            OnLoginFailure);
    }

    private void OnLoginFailure(PlayFabError error)
    {
        string errorMessage = error.GenerateErrorReport();
        string message = $"Something went wrong {errorMessage}";
        Debug.Log(message);



        //_text.SetText(message);
        //_text.color = Color.red;
    }

    private void OnLoginSuccess(LoginResult success)
    {
        string message = "Congratulations, you made successfull API call!";

        Debug.Log(message);
        //_text.SetText(message);
        //_text.color = Color.green;

        SetUserData();
        GetUserData(success.PlayFabId);
    }

    private const string _name = "Name";
    private const string _gun = "Gun";
    private void SetUserData()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string> {
                {_name, "Kyle"},
                {_gun, "Beams"}
            }
        },
         result => Debug.Log("Successfull updated user data"),
            error =>
            {
                Debug.Log("Got error setting user data Name and gun");
                Debug.Log(error.GenerateErrorReport());
            }
        );
    }

    private void GetUserData(string myPlayFabId)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() { 
            PlayFabId = myPlayFabId,
            Keys = null
        },
        result => {
            Debug.Log("Got user data:");
            if (result.Data == null || !result.Data.ContainsKey(_name))
                Debug.Log($"No {_name}");
            else 
                Debug.Log($"{_name}: " + result.Data[_name].Value);
        },
        error => {
            Debug.Log("Got error retrivinf user data:");
            Debug.Log(error.GenerateErrorReport());
        }
        );
    }

}
