using Photon.Pun;
using UnityEngine;

public class EmoteSpawn : MonoBehaviour {
    PhotonView photonView;
    PlayerInput playerInput;
    [SerializeField]
    GameObject emotePrefab;

    [SerializeField]
    Sprite[] sprites;
    [SerializeField]
    Transform emoteSpawnPoint;

    void Start() {
        playerInput = GetComponent<PlayerInput>();
        photonView = GetComponent<PhotonView>();
    }


    void Update() {
        if (playerInput.Emote_1) {
            GameObject go = Instantiate(emotePrefab, emoteSpawnPoint);
            go.GetComponentInChildren<SpriteRenderer>().sprite = sprites[0];
            photonView.RPC("RPC_Emote", RpcTarget.AllBuffered, 0);
        }
        if (playerInput.Emote_2) {
            GameObject go = Instantiate(emotePrefab, emoteSpawnPoint);
            go.GetComponentInChildren<SpriteRenderer>().sprite = sprites[1];
            photonView.RPC("RPC_Emote", RpcTarget.AllBuffered, 1);
        }
        if (playerInput.Emote_3) {
            GameObject go = Instantiate(emotePrefab, emoteSpawnPoint);
            go.GetComponentInChildren<SpriteRenderer>().sprite = sprites[2];
            photonView.RPC("RPC_Emote", RpcTarget.AllBuffered, 2);
        }
    }

    [PunRPC]
    public void RPC_Emote(int index) {
        if (!photonView.IsMine)
            return;
        Debug.Log("Emote");
        GameObject go = Instantiate(emotePrefab, emoteSpawnPoint);
        go.GetComponentInChildren<SpriteRenderer>().sprite = sprites[index];
    }
}