using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ReadyPlayerMe.AvatarLoader;
using Unity.VisualScripting;

public class PlayerSetup : MonoBehaviour {
    public List<MonoBehaviour> playerComponents = new List<MonoBehaviour>();
    public GameObject cameraPrefab;
    //public GameObject canvasObject;
    [SerializeField]
    bool _masterPlayer;
    PhotonView photonView;
    private AvatarObjectLoader avatarLoader;
    private GameObject avatar;
    [SerializeField] private GameObject defaultPlayerObject;
    string url;

    public void Start() {
        photonView = GetComponentInParent<PhotonView>();
        foreach (MonoBehaviour script in playerComponents) {
            script.enabled = photonView.IsMine;
            cameraPrefab.SetActive(photonView.IsMine);
        }
    }

    public void SetupPlayer(bool masterPlayer) {
        photonView = GetComponentInParent<PhotonView>();
        _masterPlayer = masterPlayer;
        avatarLoader = new AvatarObjectLoader();
        avatarLoader.OnCompleted += SetupAvatar;
        url = PlayerPrefs.GetString("AvatarUrl");
        avatarLoader.LoadAvatar(url);
        photonView.RPC("RPC_SetPlayerName", RpcTarget.AllBuffered, transform.parent.name, url);
    }

    [PunRPC]
    public void RPC_SetPlayerName(string _name,string avatarUrl) {
        transform.parent.name = _name;
        avatarLoader = new AvatarObjectLoader();
        avatarLoader.OnCompleted += SetupAvatar;
        avatarLoader.LoadAvatar(avatarUrl);
        photonView = GetComponentInParent<PhotonView>();
    }

    public void SetupAvatar(object sender, CompletionEventArgs args) {
        if (avatar != null) {
            Destroy(avatar);
        }
        avatar = args.Avatar;
        Animator anim =GetComponent<Animator>();
        Avatar animatorAvatar = avatar.GetComponent<Animator>().avatar;
        Destroy(avatar.GetComponent<Animator>());
        avatar.transform.Rotate(Vector3.up, 180);
        avatar.transform.parent = transform;
        avatar.transform.localPosition = Vector3.zero;
        avatar.transform.localRotation = Quaternion.identity;
        anim.avatar = animatorAvatar;
        defaultPlayerObject.SetActive(false);
    }
}