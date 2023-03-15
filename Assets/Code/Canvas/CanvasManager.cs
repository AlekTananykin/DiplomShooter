
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour, IPlayFabObserver, IPhotonObserver, IGameManagerObserver
{
    [Header("Pun")]
    [SerializeField]
    PunGameManager _photonManager = null;

    [Header("SoljarGameManager")]
    [SerializeField]
    SoljaGameManager _gameManager;

    [Header("Login Panel")]
    [SerializeField]
    private GameObject _loginPanel;
    [SerializeField]
    private TMP_InputField _palyerName_inputField;
    [SerializeField]
    private TMP_InputField _password_inputField;
    [SerializeField]
    private Button _login_Button;
    [SerializeField]
    private Button _switchToCreateAccount_Button;
    [SerializeField]
    private Button _exitGameFromLogin_Button;

    [Header("CreateAccountPanel")]
    [SerializeField]
    private GameObject _createAccountPanel;
    [SerializeField]
    private TMP_InputField _playerNameCreateAccount_inputField;
    [SerializeField]
    private TMP_InputField _passwordCreateAccount_inputField;
    [SerializeField]
    private TMP_InputField _emailCreateAccount_inputField;
    [SerializeField]
    private Button _createAccount_Button;
    [SerializeField]
    private Button _backToLogin_Button;

    [Header("MessagePanel")]
    [SerializeField]
    private LobbyMessagePanel _messagePanel;

    [Header("LobbyMainPanel")]
    [SerializeField]
    private GameObject _lobbyMainPanel;
    [SerializeField]
    private Button _createRoomPanel_Button;
    [SerializeField]
    private Button _enterRoomPanel_Button;
    [SerializeField]
    private Button _achievementPanel_Button;
    [SerializeField]
    private Button _mainPanleExitGame_Button;

    [Header("CreateRoomPanel")]
    [SerializeField]
    private GameObject _createRoomPanel;
    [SerializeField]
    private TMP_InputField _roomName_inputField;
    [SerializeField]
    private TMP_InputField _playersAccount_inputField;
    [SerializeField]
    private TMP_Dropdown _playerLocation_DropDown;
    [SerializeField]
    private Button _createRoom_Button;
    [SerializeField]
    private Button _createRoomToMainMenu_Button;

    [Header("FindRoomPanel")]
    [SerializeField]
    private GameObject _findRoomPanel;
    [SerializeField]
    private Button _findRoomToMainMenu_Button;
    [SerializeField]
    private Button _selectRoom_Button;
    [SerializeField]
    private GameObject _roomsListView;
    [SerializeField]
    private GameObject _roomsPrefab;

    [Header("Inside Room Panel")]
    [SerializeField]
    private GameObject _insideRoomPanel;
    [SerializeField]
    private Button _leaveRoom_Button;
    [SerializeField]
    private TMP_Text _roomNameHeader_Text;
    [SerializeField]
    private GameObject _playersListView;
    [SerializeField]
    private GameObject _playerDoorPrefab;
    [SerializeField]
    private Button _startGame_Button;

    [Header("Kills panel")]
    [SerializeField]
    private GameObject _achievementPanel;
    [SerializeField]
    private TMP_Text _achievementPanelKills_Text;
    [SerializeField]
    private TMP_Text _achievementPanelKilled_Text;
    [SerializeField]
    private Button _fromtAchievementoMainPanel_Button;

    [Header("Fight UI Panel")]
    [SerializeField]
    private GameObject _fightPanel;
    [SerializeField]
    private GameObject _matchResultPanel;
    [SerializeField]
    private Button _exitMatch_Button;
    [SerializeField]
    private TMP_Text _matchPanelPlayerName_text;
    [SerializeField]
    private TMP_Text _resultHeader;

    [SerializeField]
    private AudioClip _buttonSound;


    private Dictionary<string, RoomInfo> _cachedRoomList;
    private Dictionary<string, GameObject> _roomListEntries;
    private Dictionary<int, GameObject> _playerListEntries;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        _roomListEntries = new Dictionary<string, GameObject>();
        _cachedRoomList = new Dictionary<string, RoomInfo>();

        _playerListEntries = null;

        _playfabManager = new PlayFabGameManager(this);
        _photonManager.SetObserver(this);

        _gameManager.SetObserver(this);


        LoginPanelStart();
        CreateAccountPanelStart();
        MainPanelStart();
        CreateRoomPanelStart();
        FindRoomPanelStart();
        InsideRoomPanelStart();
        AchievementPanelStart();
        FightPanelStart();
    }

    private void OnDestroy()
    {
        LoginPanelDistroy();
        CreateAccountPanelDestroy();
        MainPanelDestroy();
        CreateRoomPanelDestroy();
        FindRoomPanelDestroy();
        InsideRoomPanelDestroy();
        AchievementPanelDestroy();
        FightPanelDestroy();
    }

    private void SetPanelActive(string panelName)
    {
        _loginPanel.SetActive(_loginPanel.name.Equals(panelName));
        _createAccountPanel.SetActive(
            _createAccountPanel.name.Equals(panelName));
        _lobbyMainPanel.SetActive(_lobbyMainPanel.name.Equals(panelName));
        _createRoomPanel.SetActive(_createRoomPanel.name.Equals(panelName));
        _findRoomPanel.SetActive(_findRoomPanel.name.Equals(panelName));
        _insideRoomPanel.SetActive(_insideRoomPanel.name.Equals(panelName));
        _fightPanel.SetActive(_fightPanel.name.Equals(panelName));
        _achievementPanel.SetActive(_achievementPanel.name.Equals(panelName));
    }

    private void ClearRoomListView()
    {
        foreach (GameObject entry in _roomListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        _roomListEntries.Clear();
    }

    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        Debug.Log("Update roomList");
        ClearRoomListView();

        UpdateCachedRoomList(roomList);
        UpdateRoomListView();
    }

    #region LOGIN
    void LoginPanelStart()
    {
        SetPanelActive(_loginPanel.name);
        _login_Button.onClick.AddListener(()=>
        {
            PlayButtonSound();
            Login(); 
        });

        _switchToCreateAccount_Button.onClick.AddListener(()=> {
            PlayButtonSound();
            SetPanelActive(_createAccountPanel.name);
        });

        _exitGameFromLogin_Button.onClick.AddListener(()=> {
            PlayButtonSound();
            Application.Quit();
        });
    }

    void LoginPanelDistroy()
    {
        _login_Button.onClick.RemoveAllListeners();
        _switchToCreateAccount_Button.onClick.RemoveAllListeners();
        _exitGameFromLogin_Button.onClick.RemoveAllListeners();
    }

    void Login()
    {
        _login_Button.interactable = false;
        _playerName = _palyerName_inputField.text;
        _password = _password_inputField.text;

        _playfabManager.Login(_playerName, _password);
    }

    public void OnLoginPlayFabSuccess(string message)
    {
        Debug.Log("playfab success");
        _photonManager.Login(_playerName);
    }

    public void OnLoginPlayFabFail(string error)
    {
        _messagePanel.ViewMessage($"Login playfab failed {error}");
    }

    public void OnLoginPunSuccess(string message)
    {
        Debug.Log("pun success");
        SetPanelActive(_lobbyMainPanel.name);
    }

    public void OnLoginPunFail(string message)
    {
        SetPanelActive(_loginPanel.name);
        _palyerName_inputField.text = _playerName;
        _password_inputField.text = _password;

        _messagePanel.ViewMessage("pun failed");
    }

    #endregion LOGIN
    #region CREATE_ACCOUNT
    void CreateAccountPanelStart()
    {
        _createAccount_Button.onClick.AddListener(()=>{
            PlayButtonSound();
            CreateAccount(); 
        });
       
        _backToLogin_Button.onClick.AddListener(() => {
            PlayButtonSound();
            SetPanelActive(_loginPanel.name); 
         });
    }

    void CreateAccountPanelDestroy()
    {
        _createAccount_Button.onClick.RemoveAllListeners();
        _backToLogin_Button.onClick.RemoveAllListeners();
    }

    void CreateAccount()
    {
        _email = _emailCreateAccount_inputField.text;
        _playerName = _playerNameCreateAccount_inputField.text;
        _password = _passwordCreateAccount_inputField.text;

        _playfabManager.CreateAccount(_playerName, _password, _email);
    }

    public void OnCreateAccountPlayFabSuccess(string result)
    {
        _photonManager.Login(_playerName);
    }

    public void OnCreateAccountPlayFabFail(string error)
    {
        _messagePanel.ViewMessage("Create account playfab failed");
    }

    #endregion CREATE_ACCOUNT
    #region MAIN_PANEL

    private void MainPanelStart()
    {
        _createRoomPanel_Button.onClick.AddListener(()=> {
            PlayButtonSound();
            SetPanelActive(_createRoomPanel.name); 
            });

        _enterRoomPanel_Button.onClick.AddListener(() => {
                PlayButtonSound();
                if (!PhotonNetwork.InLobby)
                {
                    PhotonNetwork.JoinLobby();
                }
                SetPanelActive(_findRoomPanel.name); 
        });
        _mainPanleExitGame_Button.onClick.AddListener(()=> {
            Debug.Log("Main Panel Exit");
            PlayButtonSound();
            Application.Quit();
        });

        _achievementPanel_Button.onClick.AddListener(()=> {
            PlayButtonSound();
            SetPanelActive(_achievementPanel.name); 
        });
    }

    private void MainPanelDestroy()
    {
        _createRoomPanel_Button.onClick.RemoveAllListeners();
        _enterRoomPanel_Button.onClick.RemoveAllListeners();
        _mainPanleExitGame_Button.onClick.RemoveAllListeners();
        _achievementPanel_Button.onClick.RemoveAllListeners();
    }

    #endregion MAIN_PANEL

    #region CREATE_ROOM_PANEL

    private void CreateRoomPanelStart()
    {
        _createRoom_Button.onClick.AddListener(()=>{
            PlayButtonSound();
            OnCreateRoomButtonClicked();  
        });

        _createRoomToMainMenu_Button.onClick.AddListener(() =>{
            PlayButtonSound();
            SetPanelActive(_lobbyMainPanel.name);
        });
    }

    private void CreateRoomPanelDestroy()
    {
        _createRoom_Button.onClick.RemoveAllListeners();
        _createRoomToMainMenu_Button.onClick.RemoveAllListeners();
    }

    private void OnCreateRoomButtonClicked()
    {
        string roomName = _roomName_inputField.text;
        roomName = (roomName.Equals(string.Empty)) ? "Room " + 
            Random.Range(1000, 10000) : roomName;

        _roomNameHeader_Text.text = roomName;

        byte maxPlayers;
        byte.TryParse(_playersAccount_inputField.text, out maxPlayers);
        maxPlayers = (byte)Mathf.Clamp(maxPlayers, 2, 8);

        _photonManager.CreateRoom(maxPlayers, roomName);
    }

    public void JoinedRoom()
    {
        Debug.Log("JoinedRoom");
      
        _cachedRoomList.Clear();
        SetPanelActive(_insideRoomPanel.name);

        if (_playerListEntries == null)
        {
            _playerListEntries = new Dictionary<int, GameObject>();
        }

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject entry = Instantiate(_playerDoorPrefab);
            entry.transform.SetParent(_playersListView.transform);
            entry.transform.localScale = Vector3.one;

            entry.GetComponent<PlayerDoor>().Initialize(p.ActorNumber, p.NickName);

            object isPlayerReady;
            if (p.CustomProperties.TryGetValue(GameUtils.PLAYER_READY, out isPlayerReady))
            {
                entry.GetComponent<PlayerDoor>().SetPlayerReady((bool)isPlayerReady);
            }

            _playerListEntries.Add(p.ActorNumber, entry);
        }

        _startGame_Button.gameObject.SetActive(CheckPlayersReady());

        //Hashtable props = new Hashtable
        //    {
        //        {AsteroidsGame.PLAYER_LOADED_LEVEL, false}
        //    };
        //PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public void LocalPlayerPropertiesUpdated()
    {
        CheckPlayersReady();
        _startGame_Button.gameObject.SetActive(CheckPlayersReady());
    }

    private bool CheckPlayersReady()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return false;
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            object isPlayerReady;
            if (player.CustomProperties.TryGetValue(GameUtils.PLAYER_READY, out isPlayerReady))
            {
                Debug.Log($"TryGetValue>>{(bool)isPlayerReady}");
                bool isReady = (bool)isPlayerReady;

                if (!isReady)
                {
                    return false;
                }
            }
            else
            {
                Debug.Log($"no isPlayerReady");
                return false;
            }
        }

        Debug.Log("all ok");

        return true;
    }

    public void LeftRoom()
    {
        SetPanelActive(_lobbyMainPanel.name);

        foreach (GameObject entry in _playerListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        _playerListEntries.Clear();
        _playerListEntries = null;
        
    }

    public void PlayerEnteredRoom(int playerNumber, string playerNickName)
    {
        GameObject entry = Instantiate(_playerDoorPrefab);
        entry.transform.SetParent(_playersListView.transform);
        entry.transform.localScale = Vector3.one;
        entry.GetComponent<PlayerDoor>().Initialize(playerNumber, playerNickName);

        _playerListEntries.Add(playerNumber, entry);

        _startGame_Button.gameObject.SetActive(CheckPlayersReady());
    }

    public void PlayerLeftRoom(int actorNumber)
    {
        Destroy(_playerListEntries[actorNumber].gameObject);
        _playerListEntries.Remove(actorNumber);

        _startGame_Button.gameObject.SetActive(CheckPlayersReady());
    }

    #endregion CREATE_ROOM_MENU
    #region FIND_ROOM_PANEL

    private void FindRoomPanelStart()
    {
        _findRoomToMainMenu_Button.onClick.AddListener(()=> { 
            PlayButtonSound();
            SetPanelActive(_lobbyMainPanel.name);
        });
    }

    private void FindRoomPanelDestroy()
    {
        _findRoomToMainMenu_Button.onClick.RemoveAllListeners();
        _selectRoom_Button.onClick.RemoveAllListeners();
    }

    #endregion FIND_ROOM_PANEL

    #region INSIDE_ROOM_PANEL
    private void InsideRoomPanelStart()
    {
        _leaveRoom_Button.onClick.AddListener( ()=> {
            PlayButtonSound();
            _photonManager.LeaveRoom(); 
        });
        _startGame_Button.onClick.AddListener(() => {
            PlayButtonSound();

            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("StartMatch", RpcTarget.All);
        });
    }

    [PunRPC]
    private void StartMatch()
    {
        SetPanelActive(_fightPanel.name);
        _matchPanelPlayerName_text.text = _playerName;
        _matchResultPanel.SetActive(false);
        _gameManager.StartMatch();
        Cursor.visible = false;
    }

    private void InsideRoomPanelDestroy()
    {
        _leaveRoom_Button.onClick.RemoveAllListeners();
    }

    #endregion INSIDE_ROOM_PANEL
    #region ACHIEVEMENT_PANEL

    private void AchievementPanelStart()
    {
        _fromtAchievementoMainPanel_Button.onClick.AddListener(()=> {
            PlayButtonSound();
            SetPanelActive(_lobbyMainPanel.name); 
        });
    }

    private void AchievementPanelDestroy()
    {
        _fromtAchievementoMainPanel_Button.onClick.RemoveAllListeners();
    }

    public void OnUpdateUserData(int killes, int killed)
    {
        _achievementPanelKills_Text.text = string.Format("Killes  {0}", killes);
        _achievementPanelKilled_Text.text = string.Format("Killed  {0}", killed);
        _killes = killes;
        _killed = killed;
    }

    #endregion
    #region FIGHT_PANEL

    private void FightPanelStart()
    {
        _exitMatch_Button.onClick.AddListener(()=> {
            PlayButtonSound();
            ExitMatch(); 
        });

        _matchResultPanel.SetActive(false);
        _fightPanel.SetActive(false);
    }

    private void FightPanelDestroy()
    {
        _exitMatch_Button.onClick.RemoveAllListeners();
    }

    private void ExitMatch()
    {
        _gameManager.ExitMatch();
        _photonManager.LeaveRoom();
        _matchResultPanel.SetActive(false);
        SetPanelActive(_lobbyMainPanel.name);
    }

    public void ShowMatchResults(int killes, int health, string headerMessage)
    {
        _resultHeader.text = headerMessage;

        _matchResultPanel.SetActive(true);
        Cursor.visible = true;

        SetMatchResults(killes, (health > 0)? 0: 1);
    }

    #endregion

    public void OnMasterClientSwitched(int playerNumber)
    {
         _startGame_Button.gameObject.SetActive(CheckPlayersReady());   
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (_cachedRoomList.ContainsKey(info.Name))
                {
                    _cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
            if (_cachedRoomList.ContainsKey(info.Name))
            {
                _cachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
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
            GameObject entry = Instantiate(_roomsPrefab);
            entry.transform.SetParent(_roomsListView.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<RoomDoor>().Initialize(info.Name,
                (byte)info.PlayerCount, info.MaxPlayers);

            _roomListEntries.Add(info.Name, entry);
        }
    }

    void PlayButtonSound()
    {
        _audioSource.PlayOneShot(_buttonSound);
    }

    public void SetMatchResults(int killes, int killed)
    {
        _killes += killes;
        _killed += killed;
        OnUpdateUserData(_killes, _killed);
        _playfabManager.SetUserData(_killes, _killed);
    }

    int _killes;
    int _killed;

    private string _email;
    private string _playerName;
    private string _password;

    private PlayFabGameManager _playfabManager;
    private AudioSource _audioSource;
}
