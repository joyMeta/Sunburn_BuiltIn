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

    [SerializeField]
    GameObject playerDMPrefab;
    [SerializeField]
    GameObject parentObject;
    [SerializeField]
    GameObject secondaryLoader;

    public void SpawnPlayer() {
        secondaryLoader.SetActive(true);
        foreach (Transform child in transform)
            playerSpawnPoints.Add(child);
        object[] data = new object[2];
        data[0] = PlayerPrefs.GetString("AvatarUrl");
        data[1] = PlayerPrefs.GetInt("MasterPlayer");

        if (Utilities.IntBoolConverter.IntToBool(PlayerPrefs.GetInt("MasterPlayer"))) {
            localPlayerObject = PhotonNetwork.Instantiate(playerPrefab.name, artistSpawnPoint.position, Quaternion.identity, 0, data);
            localPlayerObject.transform.position = artistSpawnPoint.position;
        }
        else {
            localPlayerObject = PhotonNetwork.Instantiate(playerPrefab.name, playerSpawnPoints[Random.Range(0, playerSpawnPoints.Count)].position, Quaternion.identity, 0, data);
            localPlayerObject.transform.position = playerSpawnPoints[Random.Range(0, playerSpawnPoints.Count)].position;
        }
        localPlayerObject.GetComponentInChildren<Animator>().enabled = false;
        localPlayerisMasterPlayer = Utilities.IntBoolConverter.IntToBool(PlayerPrefs.GetInt("MasterPlayer"));
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Name")) {
            localPlayerObject.name = PhotonNetwork.LocalPlayer.CustomProperties["Name"].ToString();
        }
        localPlayerObject.GetComponentInChildren<PlayerSetup>().SetupPlayer(localPlayerisMasterPlayer);
        playersInGame.Add(localPlayerObject);
        StartCoroutine(PlayerActivation());
        FindObjectOfType<ChatHandler>().SetLocalPlayer(localPlayerObject);
        FindAllPlayers();
    }

    public IEnumerator PlayerActivation() {
        localPlayerObject.SetActive(false);
        yield return new WaitForSeconds(2);
        localPlayerObject.SetActive(true);
        localPlayerObject.GetComponentInChildren<Animator>().enabled=true;
        secondaryLoader.SetActive(false);
    }

    public override void OnJoinedRoom() {
        FindAllPlayers();
    }

    public void FindAllPlayers() {
        playersInGame.Clear();
        List<PhotonView> players = FindObjectsOfType<PhotonView>().ToList();
        //foreach (PhotonView view in players) {
        //    if (!view.IsMine) {
        //        playersInGame.Add(view.gameObject);
        //        GameObject go = Instantiate(playerDMPrefab, parentObject.transform);
        //        go.GetComponent<DirectMessageSetup>().SetPlayerName(FindObjectOfType<PlayerListing>(), view.ViewID, view.Owner.NickName);
        //    }
        //}
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        Debug.Log(otherPlayer.NickName + " disconnected");
    }
}
