using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public TMP_Text roomName;
    public TMP_Text playerCount;
    LobbyManager lobbyManager;
    public GameSettings gameSettings;
    public RoomInfo roomInfo;

    private void Start() {
        lobbyManager = FindAnyObjectByType<LobbyManager>();
    }

    public void SetRoomInfo(RoomInfo _roomInfo) {
        roomInfo = _roomInfo;
        roomName.text = _roomInfo.Name;
    }

    public void UpdatePlayerCount(int roomPlayerCount,int maxPlayers) {
        playerCount.text = roomPlayerCount.ToString()+"/"+maxPlayers.ToString();
        GetComponent<Button>().interactable = (roomPlayerCount <= maxPlayers);
    }

    public void JoinRoom() {
        if (roomInfo.PlayerCount < gameSettings.MAXPLAYERS)
            lobbyManager.JoinRoom(roomName.text);
        else
            lobbyManager.RoomFull();
    }
}