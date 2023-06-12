using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AudioSetup : MonoBehaviour
{
    AudioController audioController;
    [SerializeField]
    public TMP_Text musicName;

    public void SetupAudio(string name) {
        musicName.text= name;
        audioController = FindObjectOfType<AudioController>();
    }

    public void PlayTrack() {
        audioController.PlayTrack(musicName.text);
    }
}
