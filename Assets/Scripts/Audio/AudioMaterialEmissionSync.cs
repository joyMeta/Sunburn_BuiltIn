using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMaterialEmissionSync : AudioSyncer {

    public Color[] beatColors;
    public Color restColor;
    public float beatIntensity = 75;
    public float restIntensity = 25;

    [SerializeField]
    MeshRenderer sharpieMesh;
    [SerializeField]
    private Material lampMaterial;
    int randomColor;


    private void Start() {
        lampMaterial = sharpieMesh.materials[0];
    }

    public override void OnBeat() {
        base.OnBeat();
        Color c = RandomColor();
        StopCoroutine("ColorShift");
        StartCoroutine(ColorShift(c));
    }

    public override void OnUpdate() {
        base.OnUpdate();
        if (isBeat) return;
        lampMaterial.SetColor("_EmissionColor", Color.Lerp(lampMaterial.GetColor("_EmissionColor"), restColor * restIntensity, restSmoothTime* Time.deltaTime));
    }

    private Color RandomColor() {
        if (beatColors == null || beatColors.Length == 0) return Color.white;
        randomColor = Random.Range(0, beatColors.Length);
        return beatColors[randomColor];
    }

    private IEnumerator ColorShift(Color targetColor) {
        Color currentColor = lampMaterial.GetColor("_EmissionColor");
        Color initialColor = currentColor;
        float timer = 0;

        while (currentColor != targetColor) {
            currentColor = Color.Lerp(initialColor, targetColor, timer / timeToBeat);
            timer += Time.deltaTime;
            lampMaterial.SetColor("_EmissionColor", currentColor * beatIntensity);
            yield return null;
        }
        isBeat = false;
    }
}
