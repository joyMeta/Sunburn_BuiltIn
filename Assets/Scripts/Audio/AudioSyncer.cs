using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncer : MonoBehaviour
{
    protected AudioSpectrum audioSpectrum;

    [Range(0,7)]
    public int bandIndex;
    public float timeStep;
    public float timeToBeat;
    public float restSmoothTime;
    private float timer;

    protected bool isBeat;

    protected void Start() {
        audioSpectrum=FindObjectOfType<AudioSpectrum>();
    }

    private void Update() {
        OnUpdate();
    }

    /// <summary>
    /// Inherit this to do whatever you want in Unity's update function
    /// Typically, this is used to arrive at some rest state..
    /// ..defined by the child class
    /// </summary>
    public virtual void OnUpdate() {
        if (audioSpectrum.frequencyBands[bandIndex] >= audioSpectrum.sensitivity[bandIndex])
            OnBeat();
        timer += Time.deltaTime;
    }

    public virtual void OnBeat() {
        timer = 0;
        isBeat = true;
    }
}
