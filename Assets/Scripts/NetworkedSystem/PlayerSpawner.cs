using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameSettings gameSettings;

    public Transform artistSpawnPoint;

    public GameObject playerPrefab;
    public List<Transform> playerSpawnPoints;
    public GameObject localPlayerObject;
    public List<GameObject> playersInGame = new List<GameObject>();

    bool localPlayerisMasterPlayer;

    public void Start() {
        foreach(Transform child in transform) {
            playerSpawnPoints.Add(child);
        }
        Transform spawnPoint = playerSpawnPoints[Random.Range(0, playerSpawnPoints.Count-1)];
        object[] data = new object[2];
        data[0] = PlayerPrefs.GetString("AvatarUrl");
        data[1] = PlayerPrefs.GetInt("MasterPlayer");
        if (Utilities.IntBoolConverter.IntToBool(PlayerPrefs.GetInt("MasterPlayer")))
            localPlayerObject = PhotonNetwork.Instantiate(playerPrefab.name, artistSpawnPoint.position, Quaternion.identity, 0, data);
        else
            localPlayerObject = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity, 0, data);
        localPlayerObject.GetComponentInChildren<Animator>().enabled = false;
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Name")) {
            localPlayerisMasterPlayer = (bool)PhotonNetwork.LocalPlayer.CustomProperties["MasterPlayer"];
            localPlayerObject.name = PhotonNetwork.LocalPlayer.CustomProperties["Name"].ToString();
        }
        localPlayerObject.GetComponentInChildren<PlayerSetup>().SetupPlayer(localPlayerisMasterPlayer);
        localPlayerObject.SetActive(false);
        StartCoroutine(PlayerActivation());
        playersInGame.Add(localPlayerObject);
        FindAllPlayers();
    }

    public IEnumerator PlayerActivation() {
        yield return new WaitForSeconds(2);
        localPlayerObject.SetActive(true);
        localPlayerObject.GetComponentInChildren<Animator>().enabled=true;
    }

    public override void OnJoinedRoom() {
        FindAllPlayers();
    }

    public void FindAllPlayers() {
        playersInGame.Clear();
        List<PhotonView> players = FindObjectsOfType<PhotonView>().ToList();
        foreach (PhotonView view in players) {
            if (!view.IsMine)
                playersInGame.Add(view.gameObject);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        Debug.Log(otherPlayer.NickName + " disconnected");
    }
}
