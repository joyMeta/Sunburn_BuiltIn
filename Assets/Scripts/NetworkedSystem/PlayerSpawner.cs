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
        object[] data = new object[1];
        data[0] = new Vector3(PlayerPrefs.GetFloat("Red"), PlayerPrefs.GetFloat("Green"), PlayerPrefs.GetFloat("Blue"));
        localPlayerObject = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity, 0, data);
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Name")) {
            localPlayerisMasterPlayer = (bool)PhotonNetwork.LocalPlayer.CustomProperties["MasterPlayer"];
            localPlayerObject.name = PhotonNetwork.LocalPlayer.CustomProperties["Name"].ToString();
        }
        else
            LoadFromFile(localPlayerObject);
        localPlayerObject.GetComponentInChildren<PlayerSetup>().SetupPlayer(localPlayerisMasterPlayer);
        playersInGame.Add(localPlayerObject);
        FindAllPlayers();
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

    private void LoadFromFile(GameObject localPlayer) {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = new FileStream(Utilities.saveFilePath, FileMode.OpenOrCreate);
        Vector3 colorVector = Vector3.zero;
        colorVector.x = (float)bf.Deserialize(fileStream);
        colorVector.y = (float)bf.Deserialize(fileStream);
        colorVector.z = (float)bf.Deserialize(fileStream);
        fileStream.Close();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        Debug.Log(otherPlayer.NickName + " disconnected");
    }
}
