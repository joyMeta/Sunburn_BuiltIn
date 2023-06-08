using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerInteraction : MonoBehaviour {
    PhotonView _photonView;
    PlayerInput _playerInput;
    EmoteStateMachine emoteStateMachine;
    public GameObject emotesMenu;
    Player requestedPlayer;
    [SerializeField]
    CapsuleCollider _capsuleCollider;
    private int _emoteIndex;

    [SerializeField]
    private Transform syncPosition;
    public Transform SyncPostition =>syncPosition;
    public GameObject requestedPlayerGameObject;
    Vector3 _position;

    public List<Rig> allRigs = new List<Rig>();

    void Start() {
        emoteStateMachine = GetComponent<EmoteStateMachine>();
        _playerInput= GetComponent<PlayerInput>();
        _photonView=GetComponent<PhotonView>();
    }

    void Update() {
        emotesMenu.SetActive(_playerInput.InteractionMenu);
        _capsuleCollider.enabled = _playerInput.InteractionMenu;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            requestedPlayer = other.GetComponent<PhotonView>().Owner;
            requestedPlayerGameObject = other.gameObject;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            requestedPlayer = other.GetComponent<PhotonView>().Owner;
            requestedPlayerGameObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other) {
        requestedPlayer = null;
        requestedPlayerGameObject = null;
    }

    public void SetEmote(int emoteIndex, Vector3 syncPosition) {
        _emoteIndex = emoteIndex;
        _position = syncPosition;
    }

    public void RequestEmote(int emoteIndex) {
        //Player A sends the request to the other user
        if (requestedPlayerGameObject == null) return;
        emoteStateMachine.instigator = true;
        _emoteIndex = emoteIndex;
        emoteStateMachine.requestedPlayer = requestedPlayerGameObject.GetComponent<EmoteStateMachine>();
        Player myPlayer = _photonView.Owner;
        requestedPlayerGameObject.GetComponent<PlayerResponse>().RequestEmote(myPlayer,_emoteIndex);
        _photonView.RPC("RPC_SetInstigator", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void RPC_SetInstigator() {
        emoteStateMachine.instigator = true;
    }

    public void Acceptance() {
        Debug.Log(_photonView.Owner.NickName+ "Local call");
        emoteStateMachine.instigator = true;
        emoteStateMachine.PlayEmote(_emoteIndex);
        emoteStateMachine.requestedPlayer.PlayEmote(_emoteIndex, emoteStateMachine.emoteStates.emoteHandshake.SyncPosition);
        _photonView.RPC("RPC_Acceptance",_photonView.Owner, _emoteIndex);
    }

    [PunRPC]
    public void RPC_Acceptance(int emoteIndex) {
        Debug.Log("RPC Called");
        emoteStateMachine.instigator = true;
        emoteStateMachine.PlayEmote(_emoteIndex);
        emoteStateMachine.requestedPlayer.PlayEmote(_emoteIndex, emoteStateMachine.emoteStates.emoteHandshake.SyncPosition);
    }
}