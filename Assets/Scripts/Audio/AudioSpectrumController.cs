using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct AudioGroups {
    public string name;
    public List<AudioSyncer> similarLights;
}

public class AudioSpectrumController : MonoBehaviour
{
    public List<AudioGroups> audioSyncers=new List<AudioGroups>();
    public List<Slider> sliders=new List<Slider>();

    public void Awake() {
        for (int i = 0; i < audioSyncers.Count; i++) {
            sliders[i].value = audioSyncers[i].similarLights[0].bandIndex;
        }
    }

    public void SetBand(int index) {
        foreach (AudioSyncer audioSync in audioSyncers[index].similarLights) {
            audioSync.bandIndex = (int)sliders[index].value;
        }
    }
}
