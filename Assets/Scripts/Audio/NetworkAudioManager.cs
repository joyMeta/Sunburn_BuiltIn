using UnityEngine;
using Photon.Pun;

public class NetworkAudioManager : MonoBehaviour {

    PhotonView photonView;
    AudioSource audioSource;

    public void Start() {
        photonView = GetComponent<PhotonView>();
        audioSource=GetComponent<AudioSource>();
    }

    public void PlayAudio(AudioClip audioClip,bool play) {
        Debug.Log(audioClip.name);
        byte[] clipBytes=Utilities.WavUtility.FromAudioClip(audioClip);
        photonView.RPC("RPC_PlayAudio", RpcTarget.AllBuffered, clipBytes, audioClip.name,play);
    }

    [PunRPC]
    private void RPC_PlayAudio(byte[] audioData,string fileName,bool play) {
        print(fileName);
        audioSource.clip=Utilities.WavUtility.ToAudioClip(audioData,0,fileName);
        if (play)
            audioSource.Play();
        else
            audioSource.Pause();
    }
}