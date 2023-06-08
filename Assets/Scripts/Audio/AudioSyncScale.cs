using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncScale : AudioSyncer
{
    public Vector3 beatScale;
    public Vector3 restScale;

    public override void OnBeat() {
        base.OnBeat();
        StopCoroutine("MoveToScale");
        Vector3 scale = new Vector3(beatScale.x, beatScale.y * AudioValue/10, beatScale.z);
        StartCoroutine(MoveToScale(scale));
    }

    public override void OnUpdate() {
        base.OnUpdate();
        if (isBeat) return;
        transform.localScale=Vector3.Lerp(transform.localScale,restScale, restSmoothTime*Time.deltaTime);
    }

    private IEnumerator MoveToScale(Vector3 target) {
        Vector3 currentScale = transform.localScale;
        Vector3 initialScale = currentScale;
        float timer = 0;

        while(currentScale != target) {
            currentScale = Vector3.Lerp(initialScale, target, timer / timeToBeat);
            timer += Time.deltaTime;
            transform.localScale = currentScale;
            yield return null;
        }
        isBeat = false;
    }
}
