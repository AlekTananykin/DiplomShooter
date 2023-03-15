
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabGameManager
{

    public PlayFabGameManager(IPlayFabObserver playFabObserver)
    {
        _playFabObserver = playFabObserver;
    }

    public void Login(string playerName, string password)
    {
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest()
        {
            Username = playerName,
            Password = password
        },
        result => {
            _playFabObserver.OnLoginPlayFabSuccess(result.ToString());
            _playFabId = result.PlayFabId;
            GetUserData();
         },
        error => { _playFabObserver.OnLoginPlayFabFail(error.ToString()); });
    }

    public void CreateAccount(string playerName, string password, string email)
    {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest()
            { 
                Username = playerName,
                Password = password,
                Email = email,
                RequireBothUsernameAndEmail = true
            },
        result => { _playFabObserver.OnCreateAccountPlayFabSuccess(result.ToString()); },
        error => { _playFabObserver.OnCreateAccountPlayFabFail(error.ToString()); }
        );
    }

    public void SetUserData(int killes, int killed)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() {
                {_killesKey, killes.ToString()},
                {"Killed", killed.ToString()}
            }
        },
        result => Debug.Log("Successfully updated user data"),
        error => {
            Debug.Log("Got error setting user data Ancestor to Arthur");
            Debug.Log(error.GenerateErrorReport());
        });
    }

    public void GetUserData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = _playFabId,
            Keys = null
        }, result => {
            
            Debug.Log("Got user data:");

            int killes;
            int killed;

            if (result.Data == null || !result.Data.ContainsKey(_killesKey))
                killes = 0;
            else 
                killes = int.Parse(result.Data[_killesKey].Value);

            if (result.Data == null || !result.Data.ContainsKey(_killedKey))
                killed = 0;
            else 
                killed = int.Parse(result.Data[_killedKey].Value);

            _playFabObserver.OnUpdateUserData(killes, killed);

        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });
    }

    private const string _killesKey = "Killes";
    private const string _killedKey = "Killed";

    private IPlayFabObserver _playFabObserver;
    private string _playFabId;
}
