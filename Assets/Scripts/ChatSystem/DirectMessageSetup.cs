using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DirectMessageSetup : MonoBehaviour {
    ChatHandler chatHandler;
    int playerIndex;
    [SerializeField]
    public TMP_Text playerName;
    PlayerListing playerListing;
    [SerializeField]
    Image background;
    bool thisPlayer = false;

    private void Start() {
        chatHandler = FindObjectOfType<ChatHandler>();
    }

    public void LateUpdate() {
        if(chatHandler.RecipientPlayer!=null)
            thisPlayer = chatHandler.RecipientPlayer.NickName == playerName.text;   
        background.color = chatHandler.DirectMessage&&chatHandler&&thisPlayer ? Color.red : Color.white;
    }

    public void SetPlayerName(PlayerListing _playerListing, int _playerIndex,string _playerName) {
        playerListing = _playerListing;
        playerName.text= _playerName;
        playerIndex = _playerIndex;
    }

    public void SetPlayerDetails() {
        chatHandler.SetRecipientPlayer(playerListing.playerList[playerIndex]);
    }
}