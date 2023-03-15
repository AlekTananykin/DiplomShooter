using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomEntry : MonoBehaviour
{
    public TextMeshProUGUI RoomNameText;
    public TextMeshProUGUI RoomPlayersText;
    public Button JoinRoomButton;

    private string _roomName;

    private void Start()
    {
        JoinRoomButton.onClick.AddListener(OnButtonClick);
    }

    private void OnDestroy()
    {
        JoinRoomButton.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
    }

    public void Initialize(
        string name, byte currentPlayers, byte maxPlayers)
    {
        _roomName = name;
        RoomNameText.text = name;
        RoomPlayersText.text = currentPlayers + " / " + maxPlayers;
    }
}
