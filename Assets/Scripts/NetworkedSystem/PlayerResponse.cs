using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerResponse : MonoBehaviour
{
    PhotonView _photonView;

    public GameObject requestPanel;
    Player requestedPlayer;
    int _emoteIndex;
    GameObject requestedPlayerGameObject;

    public void Start() {
        _photonView = GetComponent<PhotonView>();
    }

    public void RequestEmote(Player otherPlayer,int emoteIndex) {
        requestPanel.SetActive(true);
        requestedPlayer = otherPlayer;
        List<PhotonView> players = FindObjectsOfType<PhotonView>().ToList();
        foreach (PhotonView player in players) {
            Debug.Log(player.Owner.NickName.ToString());
            if (player.GetComponent<PhotonView>().Owner.NickName == requestedPlayer.NickName) {
                requestedPlayerGameObject = player.gameObject;
                break;
            }
        }
        GetComponent<EmoteStateMachine>().requestedPlayer = GetComponent<EmoteStateMachine>();
        _emoteIndex = emoteIndex;
        _photonView.RPC("RPC_RequestEmote",_photonView.Owner,requestedPlayer.NickName);
    }

    [PunRPC]
    public void RPC_RequestEmote(string otherPlayerNickname) {
        List<PhotonView> players = FindObjectsOfType<PhotonView>().ToList();
        foreach (PhotonView player in players) {
            Debug.Log(player.Owner.NickName.ToString());
            if (player.GetComponent<PhotonView>().Owner.NickName == otherPlayerNickname) {
                requestedPlayerGameObject = player.gameObject;
                break;
            }
        }
        GetComponent<EmoteStateMachine>().requestedPlayer = GetComponent<EmoteStateMachine>();
        requestedPlayer = requestedPlayerGameObject.GetComponent<PhotonView>().Owner;
        requestPanel.SetActive(true);
    }

    public void AcceptInvitation() {
        FindObjectOfType<PlayerSpawner>().FindAllPlayers();
        requestPanel.SetActive(false);
        PlayerInteraction playerInteraction = requestedPlayerGameObject.GetComponent<PlayerInteraction>();
        playerInteraction.Acceptance();
    }
}
