using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Xml;
using System.Collections;

public class ChatHandler : MonoBehaviour {
    PlayerInteraction playerInteraction;
    PhotonView _photonView;
    public InputField inputField;

    bool directMessage = false;
    public bool DirectMessage=>directMessage;
    Player recipientPlayer;
    public Player RecipientPlayer=>recipientPlayer;

    [SerializeField]
    Text chatText;
    PlayerSpawner playerSpawner;

    bool canMessage;

    int messageCounter;
    [SerializeField]
    int permissibleMessages=5;
    [SerializeField]
    float messageCooldown=5;
    float timeElapsed;

    [SerializeField]
    GameObject softBanWarning;

    string csvLink;

    public DataList questionData = new DataList();

    bool inChat;
    GameObject localPlayer;

    [SerializeField]
    Animator chatAnimator;

    public void Start() {
        playerInteraction=GetComponent<PlayerInteraction>();
        _photonView = GetComponent<PhotonView>();
        playerSpawner = FindObjectOfType<PlayerSpawner>();
        //softBanWarning.SetActive(false);
        if(PhotonNetwork.IsMasterClient)
            SyncFAQ();
    }

    public void SyncFAQ() {
        csvLink = FindObjectOfType<PlayerListing>().csvLink;
        _photonView.RPC("RPC_GetCSVLink", RpcTarget.AllBuffered, csvLink);
        StartCoroutine(Utilities.CSVDownloader.DownloadData(csvLink, DownloadComplete));
    }

    [PunRPC]
    public void RPC_GetCSVLink(string _csvLink) {
        Debug.Log(_csvLink);
        csvLink = _csvLink;
        StartCoroutine(Utilities.CSVDownloader.DownloadData(csvLink, DownloadComplete));
    }

    public void DownloadComplete(string _data) {
        Debug.Log(_data);
        if (_data == null)
            return;
        string[] data = _data.Split(new string[] {",", "\n" },System.StringSplitOptions.None);
        int tableSize = data.Length / 3 - 1;

        questionData.data = new DataClass[tableSize];
        for (int i = 0; i < tableSize; i++) {
            questionData.data[i] = new DataClass();
            questionData.data[i].index = int.Parse(data[3 * (i + 1)]);
            questionData.data[i].question = data[3 * (i + 1) + 1];
            questionData.data[i].answer = data[3 * (i + 1) + 1];
        }
    }

    public void SetLocalPlayer(GameObject player) {
        localPlayer = player;
    }

    private void Update() {
        //timeElapsed += Time.deltaTime;
        //canMessage = timeElapsed < messageCooldown && messageCounter < permissibleMessages;
        //if (timeElapsed > messageCooldown) {
        //    timeElapsed = 0;
        //    messageCounter = 0;
        //}
        chatAnimator.SetBool("Open", playerInteraction.SendChat);
        if (localPlayer == null)
            return;
        localPlayer.GetComponentInChildren<PlayerInput>().enabled = !chatAnimator.GetBool("Open");
    }

    public void SetRecipientPlayer(Player _recipientPlayer) {
        if (_recipientPlayer==recipientPlayer&& directMessage)
            directMessage = false;
        else if(_recipientPlayer!=recipientPlayer&&directMessage) {
            recipientPlayer = _recipientPlayer;
            directMessage = true;
        }
        else {
            directMessage = true;
            recipientPlayer = _recipientPlayer;
        }
    }

    public void SendString() {
        //if (!canMessage) {
        //    StopCoroutine(SoftBan());
        //    StartCoroutine(SoftBan());
        //    return;
        //}
        //messageCounter++;
        inChat = false;
        if (directMessage) {
            _photonView.RPC("RPC_SendDirectMessage", recipientPlayer, playerSpawner.localPlayerObject.GetComponentInChildren<PhotonView>().Owner.NickName, inputField.text);
            chatText.text += new string("\n" + _photonView.Owner.NickName + " : " + inputField.text);
        }
        else
            _photonView.RPC("RPC_BroadcastMessage", RpcTarget.AllBuffered, playerSpawner.localPlayerObject.GetComponentInChildren<PhotonView>().Owner.NickName, inputField.text);
        inputField.text = "";
    }

    IEnumerator SoftBan() {
        softBanWarning.SetActive(true);
        yield return new WaitUntil(() => canMessage);
        softBanWarning.SetActive(false);
    }

    [PunRPC]
    public void RPC_SendDirectMessage(string nickname, string message) {
        chatText.text += new string("\n" + nickname + " : " + message);
    }

    [PunRPC]
    public void RPC_BroadcastMessage(string nickname, string message) {
        chatText.text += new string("\n" + nickname + " : " + message);
    }

    public void InChat(bool value) {
        inChat = value;
    }
}