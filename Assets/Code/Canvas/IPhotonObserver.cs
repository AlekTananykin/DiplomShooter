using Photon.Realtime;
using System.Collections.Generic;

public interface IPhotonObserver
{
    void OnLoginPunSuccess(string message);
    void OnLoginPunFail(string message);

    
    void JoinedRoom();
    void LeftRoom();

    void PlayerEnteredRoom(int playerNumber, string playerNickName);
    
    void PlayerLeftRoom(int actorNumber);

    void OnMasterClientSwitched(int actorNumber);
    public void UpdateRoomList(List<RoomInfo> roomList);
}