
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class PunGameManager: MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void SetObserver(IPhotonObserver photonObserver) 
    {
        _photonObserver = photonObserver;
    }

    public void Login(string playerName)
    {
        if (!playerName.Equals(""))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.LogError("Player Name is invalid.");
        }
    }

    public  void GetCustomRoomList()
    {
        string sqlLobbyFilter = "";
        TypedLobby sqlLobby = new TypedLobby("customSqlLobby", LobbyType.Default);
        PhotonNetwork.GetCustomRoomList(sqlLobby, sqlLobbyFilter);
    }


    public void CreateRoom(byte maxPlayers, string roomName)
    {
        RoomOptions options = new RoomOptions 
        { 
            MaxPlayers = maxPlayers, PlayerTtl = 10000 
        };

        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    public void LeaveRoom()
    {
        Debug.Log("LeaveRoom");
        PhotonNetwork.LeaveRoom();
    }

    public override void OnConnectedToMaster()
    {
        _photonObserver.OnLoginPunSuccess("On connected to master");
        Debug.Log("OnConnectedToMaster");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
    }

    public override void OnJoinedRoom()
    {
        // joining (or entering) a room invalidates any cached lobby room list (even if LeaveLobby was not called due to just joining a room)
        _photonObserver.JoinedRoom();
    }

    public override void OnLeftRoom()
    {
        _photonObserver.LeftRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        _photonObserver.PlayerEnteredRoom(newPlayer.ActorNumber, newPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        _photonObserver.PlayerLeftRoom(otherPlayer.ActorNumber);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);

        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            _photonObserver.OnMasterClientSwitched(newMasterClient.ActorNumber);
        }
    }

    public void LoadScene(string sceneName)
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        PhotonNetwork.LoadLevel(sceneName);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        _photonObserver.UpdateRoomList(roomList);
    }


    private IPhotonObserver _photonObserver;
}
