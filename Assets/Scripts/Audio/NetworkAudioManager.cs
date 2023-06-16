using UnityEngine;
using Photon.Pun;
using System.IO;

public class NetworkAudioManager : MonoBehaviour {

    PhotonView photonView;
    AudioSource audioSource;

    public void Start() {
        photonView = GetComponent<PhotonView>();
        audioSource=GetComponent<AudioSource>();
    }

    public void PlayAudio(AudioClip audioClip,bool play) {
        byte[] clipBytes=File.ReadAllBytes(audioClip.name);
        photonView.RPC("RPC_PlayAudio", RpcTarget.AllBuffered, clipBytes, audioClip.name,play);
    }

    [PunRPC]
    private void RPC_PlayAudio(byte[] audioData,string fileName,bool play) {
        print(fileName);
        using (FileStream bytoToAudio=File.Create(fileName)) {
            bytoToAudio.Write(audioData,0,audioData.Length);
        }
        audioSource.clip= Utilities.WavUtility.ToAudioClip(audioData,0,fileName);
        if (play)
            audioSource.Play();
        else
            audioSource.Pause();
    }
}