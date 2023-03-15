using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanel : MonoBehaviourPunCallbacks
{
    [Header("Login Panel")]
    public GameObject LoginPanel;

    public TMP_InputField PlayerNameInput;

    [Header("Selection Panel")]
    public GameObject SelectionPanel;

    [Header("Create Room Panel")]
    public GameObject CreateRoomPanel;

    public TMP_InputField RoomNameInputField;
    public TMP_InputField MaxPlayersInputField;
    public Button CreateRoomButton;

    [Header("Room List Panel")]
    public GameObject RoomListPanel;

    public GameObject RoomListContent;
    public GameObject RoomListEntryPrefab;

    [Header("Inside Room Panel")]
    public GameObject InsideRoomPanel;

    public GameObject PlayersPanel;

    public Button StartGameButton;
    public GameObject PlayerListEntryPrefab;

    private Dictionary<string, GameObject> _roomListEntries;
    private Dictionary<string, RoomInfo> _cachedRoomList;
    private Dictionary<int, GameObject> _playerListEntries;

    string gameVersion = "1";

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        _roomListEntries = new Dictionary<string, GameObject>();
        _cachedRoomList = new Dictionary<string, RoomInfo>();

        PlayerNameInput.text = "Player " + Random.Range(1000, 10000);

        RoomNameInputField.text = "Room " + 
            UnityEngine.Random.Range(1000, 10000);

        MaxPlayersInputField.text = "5";
        CreateRoomButton.onClick.AddListener(OnCreateRoomButtonClicked);

        SetActivePanel(LoginPanel.name);
    }

    private void OnDestroy()
    {
        CreateRoomButton.onClick.RemoveAllListeners();
    }

    private void SetActivePanel(string panelName)
    {
        LoginPanel.SetActive(panelName.Equals(LoginPanel.name));
        SelectionPanel.SetActive(panelName.Equals(SelectionPanel.name));
        RoomListPanel.SetActive(panelName.Equals(RoomListPanel.name));
        CreateRoomPanel.SetActive(panelName.Equals(CreateRoomPanel.name));
        InsideRoomPanel.SetActive(panelName.Equals(InsideRoomPanel.name));
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        this.SetActivePanel(SelectionPanel.name);

       
        //Connect();
        Debug.Log("OnConnectedToMaster was called. ");

        //if (!PhotonNetwork.InLobby)
         //   PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        _cachedRoomList.Clear();
        ClearRoomListView();
        Debug.Log("join lobby");
    }

    public override void OnLeftLobby()
    {
        _cachedRoomList.Clear();
        ClearRoomListView();
        Debug.Log("OnLeftLobby");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        //SetActivePanel(SelectionPanel.name);
        Debug.Log("OnCreateRoomFailed");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //SetActivePanel(SelectionPanel.name);
        Debug.Log("OnJoinRoomFailed");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomName = "Room " + Random.Range(1000, 10000);

        RoomOptions options = new RoomOptions { MaxPlayers = 8 };

        PhotonNetwork.CreateRoom(roomName, options, null);
        Debug.Log("OnJoinRandomFailed");
    }

    public override void OnJoinedRoom()
    {
        // joining (or entering) a room invalidates any cached lobby room list (even if LeaveLobby was not called due to just joining a room)
        _cachedRoomList.Clear();

        SetActivePanel(InsideRoomPanel.name);
        
        if (_playerListEntries == null)
        {
            _playerListEntries = new Dictionary<int, GameObject>();
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject entry = Instantiate(PlayerListEntryPrefab);
            entry.transform.SetParent(PlayersPanel.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<PlayerEntry>().Initialize(player.ActorNumber, player.NickName);

            object isPlayerReady = true;
            //if (player.CustomProperties.TryGetValue(AsteroidsGame.PLAYER_READY, out isPlayerReady))
            {
                entry.GetComponent<PlayerEntry>().SetPlayerReady((bool)isPlayerReady);
            }

            _playerListEntries.Add(player.ActorNumber, entry);
        }

        //StartGameButton.gameObject.SetActive(CheckPlayersReady());
        /*
        Hashtable props = new Hashtable
        {
            {AsteroidsGame.PLAYER_LOADED_LEVEL, false}
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        */
    }

    public override void OnLeftRoom()
    {
        
        SetActivePanel(SelectionPanel.name);

        foreach (GameObject entry in _playerListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        _playerListEntries.Clear();
        _playerListEntries = null;
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.ActorNumber} has been entered to the room");
        /*
        GameObject entry = Instantiate(PlayerListEntryPrefab);
        entry.transform.SetParent(InsideRoomPanel.transform);
        entry.transform.localScale = Vector3.one;
        entry.GetComponent<PlayerListEntry>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);

        playerListEntries.Add(newPlayer.ActorNumber, entry);

        StartGameButton.gameObject.SetActive(CheckPlayersReady());
        */
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        /*Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
        playerListEntries.Remove(otherPlayer.ActorNumber);

        StartGameButton.gameObject.SetActive(CheckPlayersReady());
        */
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            //StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        /*
        if (playerListEntries == null)
        {
            playerListEntries = new Dictionary<int, GameObject>();
        }

        GameObject entry;
        if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
        {
            object isPlayerReady;
            if (changedProps.TryGetValue(AsteroidsGame.PLAYER_READY, out isPlayerReady))
            {
                entry.GetComponent<PlayerListEntry>().SetPlayerReady((bool)isPlayerReady);
            }
        }

        StartGameButton.gameObject.SetActive(CheckPlayersReady());
        */
    }

    #region
    public void OnCreateRoomButtonClicked()
    {
        string roomName = RoomNameInputField.text;
        if (roomName.Length == 0)
        {
            roomName = "Room" +
                UnityEngine.Random.Range(1000, 10000);
            RoomNameInputField.text = roomName;
        }

        byte maxPlayers;
        byte.TryParse(MaxPlayersInputField.text, out maxPlayers);
        maxPlayers = (byte)Mathf.Clamp(maxPlayers, 2, 8);

        RoomOptions options = new RoomOptions { 
            MaxPlayers = maxPlayers,
            PlayerTtl = 10000
        };
        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("Room has been created");

        //if (PhotonNetwork.InRoom)
         //   PhotonNetwork.LeaveRoom();

        SetActivePanel(InsideRoomPanel.name);

        //PhotonNetwork.JoinOrCreateRoom()

    }
    #endregion

    #region 

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        ClearRoomListView();
        UpdatedCachedRoomList(roomList);
        UpdateRoomListView();

        Debug.Log("OnRoomListUpdate");
    }
    public void OnRoomListButtonClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        SetActivePanel(RoomListPanel.name);
    }

    private void ClearRoomListView()
    {
        foreach (GameObject entry in _roomListEntries.Values)
        {
            Destroy(entry.gameObject);
        }
        _roomListEntries.Clear();
    }

    private void UpdatedCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            Debug.Log(info.Name);

            if (!info.IsOpen || info.IsVisible || info.RemovedFromList)
            {
                if (_cachedRoomList.ContainsKey(info.Name))
                {
                    _cachedRoomList.Remove(info.Name);
                }
                continue;
            }

            if (_cachedRoomList.ContainsKey(info.Name))
            {
                _cachedRoomList[info.Name] = info;
            }
            else
            {
                _cachedRoomList.Add(info.Name, info);
            }
        }
    }

    private void UpdateRoomListView()
    {
        foreach (RoomInfo info in _cachedRoomList.Values)
        {
            GameObject entry = Instantiate(RoomListEntryPrefab);
            entry.transform.SetParent(RoomListContent.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<RoomEntry>().Initialize(
                info.Name, (byte)info.PlayerCount, info.MaxPlayers);

            _roomListEntries.Add(info.Name, entry);
        }
    }
    #endregion
    #region
    public void OnStartGameButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        PhotonNetwork.LoadLevel("Scenes/SampleScene");
    }
    #endregion

    public void OnLoginButtonClicked()
    {
        string playerName = PlayerNameInput.text;

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

    public void OnCreateRoomPanelButtonClicked()
    {
        SetActivePanel(CreateRoomPanel.name);
    }

    public void OnSelectionPanelButtonClicked()
    {
        SetActivePanel(SelectionPanel.name);
    }
}
