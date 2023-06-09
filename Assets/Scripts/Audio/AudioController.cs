using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour {
    AudioSpectrum audioSpectrum;

    public Slider[] sliders;
    private void Awake() {
        audioSpectrum = FindObjectOfType<AudioSpectrum>();
        for(int i=0;i<audioSpectrum.sensitivity.Length;i++) {
            sliders[i].value= audioSpectrum.sensitivity[i];
        }
    }

    public void SetSensitivity(int index) {
        audioSpectrum.sensitivity[index] = sliders[index].value;
    }
}