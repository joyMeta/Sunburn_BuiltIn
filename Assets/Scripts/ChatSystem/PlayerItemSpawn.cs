using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemSpawn : MonoBehaviour
{
    [SerializeField]
    GameObject playerItemObject;

    [SerializeField]
    GameObject parentObject;

    public void Start() {
        foreach (KeyValuePair<int,Player> player in PlayerListing.Instance.playerList) {
            if (!player.Value.IsLocal) {
                Debug.Log(player.Key);
                GameObject go = Instantiate(playerItemObject, parentObject.transform);
                go.GetComponent<DirectMessageSetup>().SetPlayerName(PlayerListing.Instance, player.Key, player.Value.NickName);
            }
        }
    }
}
