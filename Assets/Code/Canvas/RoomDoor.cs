
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomDoor : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _roomName_Text;
    [SerializeField]
    private TMP_Text _roomPlayers_Text;
    [SerializeField]
    private Button _selectRoombutton;

    private string _roomName;


    void Start()
    {
        _selectRoombutton.onClick.AddListener(()=> {

            if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.LeaveLobby();
            }
            PhotonNetwork.JoinRoom(_roomName);
        });
    }

    public void Initialize(string roomName, byte currentPlayers, byte maxPlayers)
    {
        _roomName = roomName;
        _roomName_Text.text = _roomName;
        _roomPlayers_Text.text = string.Format("{0}/{1}", currentPlayers, maxPlayers);
    }
}
