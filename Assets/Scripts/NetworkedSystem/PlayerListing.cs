using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerListing : MonoBehaviourPunCallbacks
{
    public PlayerDictionary playerList=new PlayerDictionary ();
    public static PlayerListing Instance;
    [SerializeField]
    TMP_InputField inputField;
    public string csvLink;

    private void Start() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void UpdateLink() {
        csvLink = inputField.text;
    }

    public override void OnJoinedRoom() {
        UpdatePlayerList();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        UpdatePlayerList();
    }

    public void UpdatePlayerList() {
        playerList.Clear();
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players) {
            playerList.Add(player.Key, player.Value);
        }
    }
}
