using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomManagerView : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private byte _maxPlayers = 4;

    [SerializeField]
    GameObject _inputText;

    private void Start()
    {
    }

    private void OnDestroy()
    {
    }

    public void OnClick_CreateRoom()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogError("Photon is not connected. ");
            return;
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = _maxPlayers;
        
        PhotonNetwork.JoinOrCreateRoom(
            _inputText.GetComponent<TMP_InputField>().text, 
            roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("OnCreatedRoom success");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("OnCreatedRoom failed");
    }

}
