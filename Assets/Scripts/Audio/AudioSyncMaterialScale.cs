using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncMaterialScale : AudioSyncer {

    public Vector3 beatScale;
    public Vector3 restScale;

    Material mat;
    [SerializeField]
    string mapString;

    [SerializeField]
    int materialIndex = 0;

    private void Start() {
        mat = GetComponent<MeshRenderer>().materials[materialIndex];
    }

    public override void OnBeat() {
        base.OnBeat();
        StopCoroutine("MoveToScale");
        StartCoroutine(MoveToScale(beatScale));
    }

    public override void OnUpdate() {
        base.OnUpdate();
        if (isBeat) return;
        mat.SetTextureScale(mapString, Vector3.Lerp(transform.localScale, restScale, restSmoothTime * Time.deltaTime));
    }

    private IEnumerator MoveToScale(Vector3 target) {
        Vector3 currentScale = transform.localScale;
        Vector3 initialScale = currentScale;
        float timer = 0;

        while (currentScale != target) {
            currentScale = Vector3.Lerp(initialScale, target, timer / timeToBeat);
            timer += Time.deltaTime;
            mat.SetTextureScale(mapString, currentScale);

            yield return null;
        }
        isBeat = false;
    }
}