using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomInputField;
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    public TMP_Text roomName;

    public RoomItem roomItemPrefab;
    public List<RoomItem> roomItemsList=new();
    public Transform roomItemParent;
    public float pollingTime = 0.5f;
    float nextUpdate;

    public List<PlayerItem> playerItemsList = new();
    public PlayerItem playerItemPrefab;
    public PlayerItem localPlayerPrefab;
    public Transform playerItemParent;
    public Transform localPlayerParent;
    public GameSettings gameSettings;
    [SerializeField]
    GameObject roomFull;
    public GameObject playButton;
    [SerializeField]
    TMP_InputField linkInputField;


    private void Start() {
        PhotonNetwork.JoinLobby();
        Deactivate();
    }

    public void CreateRoom() {
        if (roomInputField.text.Length > 0) {
            PhotonNetwork.CreateRoom(roomInputField.text,new RoomOptions() {MaxPlayers= gameSettings.MAXPLAYERS, BroadcastPropsChangeToAll=true,EmptyRoomTtl=2000});
            PlayerPrefs.SetInt("MasterPlayer", 1);
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        Debug.Log(message);
    }

    public override void OnJoinedRoom() {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        if (Time.time >= nextUpdate) {
            foreach (RoomInfo room in roomList) {
                if (room.RemovedFromList) {
                    int index=roomItemsList.FindIndex(x=>x.roomInfo.Name==room.Name);
                    if (index != -1) {
                        Destroy(roomItemsList[index].gameObject);
                        roomItemsList.RemoveAt(index);
                    }
                }
                else {
                    RoomItem newRoom = Instantiate(roomItemPrefab, roomItemParent);
                    newRoom.gameSettings = gameSettings;
                    newRoom.SetRoomInfo(room);
                    newRoom.UpdatePlayerCount(room.PlayerCount, gameSettings.MAXPLAYERS);
                    roomItemsList.Add(newRoom);
                }
            }
            nextUpdate = Time.time + pollingTime;
        }
    }

    private void Update() {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= gameSettings.MINPLAYERS)
            playButton.SetActive(true);
        else
            playButton.SetActive(false);
    }

    public void JoinRoom(string roomName) {
        linkInputField.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        PhotonNetwork.JoinRoom(roomName);
        PlayerPrefs.SetInt("MasterPlayer",0);
    }

    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinLobby();
    }

    public override void OnLeftRoom() {
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
    }

    public void UpdatePlayerList() {
        if (playerItemsList.Count > 0) {
            foreach (PlayerItem item in playerItemsList) {
                Destroy(item.gameObject);
            }
        }
        playerItemsList.Clear();
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players) {
            PlayerItem playerItem = new();
            playerItem = Instantiate(playerItemPrefab, playerItemParent);
            playerItem.SetPlayerInfo(player.Value);
            playerItemsList.Add(playerItem); 
            playerItem.masterPlayer = PhotonNetwork.IsMasterClient;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        UpdatePlayerList();
    }

    public void RoomFull() {
        roomFull.SetActive(true);
    }

    public void Deactivate() {
        roomFull.SetActive(false);
    }

    public void PlayButton() {
        PhotonNetwork.LoadLevel("MainScene");
    }
}