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
        if (!photonView.IsMine)
            return;
        if (playerInput.Emote_1) {
            GameObject go = Instantiate(emotePrefab, emoteSpawnPoint.position + new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f)), Quaternion.identity);
            go.GetComponentInChildren<SpriteRenderer>().sprite = sprites[0];
            photonView.RPC("RPC_Emote", RpcTarget.AllBuffered, photonView.ViewID, 0);
        }
        if (playerInput.Emote_2) {
            GameObject go = Instantiate(emotePrefab, emoteSpawnPoint.position + new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f)), Quaternion.identity);
            go.GetComponentInChildren<SpriteRenderer>().sprite = sprites[1];
            photonView.RPC("RPC_Emote", RpcTarget.AllBuffered, photonView.ViewID, 1);
        }
        if (playerInput.Emote_3) {
            GameObject go = Instantiate(emotePrefab, emoteSpawnPoint.position + new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f)), Quaternion.identity);
            go.GetComponentInChildren<SpriteRenderer>().sprite = sprites[2];
            photonView.RPC("RPC_Emote", RpcTarget.AllBuffered, photonView.ViewID, 2);
        }
    }
    [PunRPC]
    public void RPC_Emote(int photonID, int index) {
        if (photonView.ViewID != photonID)
            return;
        Debug.Log("Emote");
        GameObject go = Instantiate(emotePrefab, emoteSpawnPoint.position+new Vector3(Random.Range(-1f,1f),0.5f,Random.Range(-1f,1f)),Quaternion.identity);
        go.GetComponentInChildren<SpriteRenderer>().sprite = sprites[index];
    }
}