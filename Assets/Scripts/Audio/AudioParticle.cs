using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioParticle : AudioSyncer {

    ParticleSystem flameParticleSystem;

    public void Start() {
        base.Start();
        flameParticleSystem= GetComponent<ParticleSystem>();
    }

    public override void OnUpdate() {
        base.OnUpdate();
        if (isBeat) return;
        flameParticleSystem.Stop();
    }

    public override void OnBeat() {
        base.OnBeat();
        flameParticleSystem.Play();
    }

}