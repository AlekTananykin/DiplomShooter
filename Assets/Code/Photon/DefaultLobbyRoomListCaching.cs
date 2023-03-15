
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class DefaultLobbyRoomListCaching : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;
    public GameObject roomListingPrefab;

    private void Start()
    {
        Debug.Log("Room Listin start");
    }

    private TypedLobby _customLobby = new TypedLobby(
        "customLobby", LobbyType.Default);

    private Dictionary<string, RoomInfo> _cachedRoomList = 
        new Dictionary<string, RoomInfo>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        RemoveRoomListing(roomList);

        foreach (RoomInfo room in roomList)
        {
            ListRoom(room);
        }
    }

    private void ListRoom(RoomInfo room)
    {
        Debug.Log(room.Name);
        if (room.IsOpen && room.IsVisible)
        {
            GameObject tempListing = Instantiate(roomListingPrefab, _content);
            //RoomListing roomListin = tempListing.GetComponent<RoomListing>();

        }
    }

    private void RemoveRoomListing(List<RoomInfo> roomList)
    {
        while (_content.childCount != 0)
        {
            Destroy(_content.GetChild(0).gameObject);
        }
    }
}
