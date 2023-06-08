using TMPro;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    GameSettings gameSettings;

    public TMP_Text playerName;
    public Image playerColorImage;
    public Hashtable playerProperties = new Hashtable();

    Player player;
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;
    public bool masterPlayer=false;

    //private string savePath;

    public void SetPlayerInfo(Player _player) {
        playerName.text= _player.NickName;
        player= _player;
        playerProperties["Name"]= _player.NickName;
        player.CustomProperties["Name"] = _player.NickName;
        if (!_player.IsLocal)
            return;
        if (playerProperties.ContainsKey("Color")) {
            Vector3 colorVector = (Vector3)playerProperties["Color"];
            redSlider.value = colorVector.x;
            greenSlider.value = colorVector.y;
            blueSlider.value = colorVector.z;
        }
        else
            playerProperties["Color"] = new Vector3(redSlider.value, greenSlider.value, blueSlider.value);
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void ColorChange() {
        playerProperties["Color"] = new Vector3(redSlider.value, greenSlider.value, blueSlider.value);
        playerProperties["MasterPlayer"] = masterPlayer;
        playerColorImage.color = new Color(redSlider.value, greenSlider.value, blueSlider.value, 1);
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        FlushToFile();
    }

    public void FlushToFile() {
        Debug.Log("saving to file");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = new FileStream(Utilities.saveFilePath, FileMode.OpenOrCreate);
        bf.Serialize(fileStream, redSlider.value);
        bf.Serialize(fileStream, greenSlider.value);
        bf.Serialize(fileStream, blueSlider.value);
        bf.Serialize(fileStream, masterPlayer);
        fileStream.Close();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) {
        if (targetPlayer == player && targetPlayer.CustomProperties.ContainsKey("Color")) {
            Vector3 playerColorVector =(Vector3) targetPlayer.CustomProperties["Color"];
            playerColorImage.color = new Color(playerColorVector.x, playerColorVector.y, playerColorVector.z, 1);
            playerProperties["Color"] = playerColorVector;
            playerProperties["MasterPlayer"] = targetPlayer.CustomProperties["MasterPlayer"];
        }
    }
}
