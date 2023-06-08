using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncer : MonoBehaviour
{
    public float bias;
    public float timeStep;
    public float timeToBeat;
    public float restSmoothTime;
    public float AudioValue => audioValue;

    private float previousAudioValue;
    private float audioValue;
    private float timer;

    protected bool isBeat;

    private void Update() {
        OnUpdate();
    }

    /// <summary>
    /// Inherit this to do whatever you want in Unity's update function
    /// Typically, this is used to arrive at some rest state..
    /// ..defined by the child class
    /// </summary>
    public virtual void OnUpdate() {
        previousAudioValue = audioValue;
        audioValue = AudioSpectrum.spectrumValue;
        audioValue=Mathf.Clamp(audioValue,0,float.MaxValue);

        if(audioValue>=bias)
            OnBeat();
        timer += Time.deltaTime;
    }

    public virtual void OnBeat() {
        timer = 0;
        isBeat = true;
    }
}
