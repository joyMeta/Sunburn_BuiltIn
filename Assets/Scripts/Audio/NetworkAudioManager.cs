using UnityEngine;
using Photon.Pun;

public class NetworkAudioManager : MonoBehaviour {

    public void Start() {
        FindObjectOfType<AudioSpectrum>().recorderAudioSource = GetComponent<AudioSource>();
    }
}