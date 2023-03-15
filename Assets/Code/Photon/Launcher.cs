using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TextMeshProUGUI _textToChange;

    string gameVersion = "1";

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        Connect();
    }

    
    private void Dispose()
    {
        
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    private void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            //PhotonNetwork.JoinRandomRoom();
        }
        else 
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log("OnConnectedToMaster was called. ");

        _textToChange.SetText(_connectLabel);
        _isConnected = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log("Photon Disconnected was called. ");
        _textToChange.SetText(_disconnectLabel);
        _isConnected = false;
    }

    public void Switch()
    {
        if (_isConnected)
            Disconnect();
        else
            Connect();
    }

    private string _connectLabel = "Photon Connected";
    private string _disconnectLabel = "Photon Disconnected";
    private bool _isConnected = false;
}
