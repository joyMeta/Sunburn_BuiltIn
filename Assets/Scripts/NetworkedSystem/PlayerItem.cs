using TMPro;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerItem : MonoBehaviourPunCallbacks {
    GameSettings gameSettings;
    public Hashtable playerProperties = new Hashtable();

    Player player;
    public bool masterPlayer = false;

    [SerializeField]
    GameObject localPlayerRig;
    public RuntimeAnimatorController animatorController;

    [SerializeField]
    TMP_Text playerName;

    public void SetPlayerInfo(Player _player) {
        playerProperties["MasterPlayer"] = true;
        playerProperties["AvatarURL"] = PlayerPrefs.GetString("AvatarUrl");
        playerName.text = _player.NickName;
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void SetPlayerRig(GameObject playerAvatar) {
        localPlayerRig.SetActive(false);
        playerAvatar.GetComponent<Animator>().runtimeAnimatorController = animatorController;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) {
        if (targetPlayer == player) {
            playerName.text = PlayerPrefs.GetString("Name");
            playerProperties["AvatarURL"] = PlayerPrefs.GetString("AvatarUrl");
            playerProperties["MasterPlayer"] = targetPlayer.CustomProperties["MasterPlayer"];
        }
    }
}