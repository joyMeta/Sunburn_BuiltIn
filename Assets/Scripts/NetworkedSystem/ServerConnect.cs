using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class ServerConnect : MonoBehaviourPunCallbacks
{
    public TMP_InputField  usernameInputField;
    public TMP_Text buttonText;

    public void ConnectToServer() {
        if (usernameInputField.text.Length > 0) {
            PhotonNetwork.NickName= usernameInputField.text;
            PlayerPrefs.SetString("Name", usernameInputField.text);
            buttonText.text = "Connecting...";
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LobbyScene");
    }
}
