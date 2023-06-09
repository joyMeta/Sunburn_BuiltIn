using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLightSyncer : AudioSyncer
{
    public Color[] beatColors;
    public Color restColor;

    public float beatEdge = 2;
    public float restEdge = 0;

    public float beatIntensity = 75;
    public float restIntensity = 25;

    [SerializeField]
    private Light sourceLight;
    [SerializeField]
    MeshRenderer sharpieMesh;
    [SerializeField]
    private Material lampMaterial;
    [SerializeField]
    MeshRenderer lightConeMesh;
    [SerializeField]
    private Material lightConeMaterial;
    int randomColor;

    bool noLightCone;
    [SerializeField]
    string colorName = "_EmissionColor";

    private void Start() {
        lampMaterial=sharpieMesh.materials[0];
        noLightCone = lightConeMesh == null;
        if (!noLightCone)
            lightConeMaterial = lightConeMesh.materials[0];
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
        sourceLight.color = Color.Lerp(sourceLight.color, restColor, restSmoothTime * Time.deltaTime);
        sourceLight.intensity=Mathf.Lerp(sourceLight.intensity,restIntensity,restSmoothTime * Time.deltaTime);
        lampMaterial.SetColor(colorName, sourceLight.color*restIntensity);
        if (!noLightCone)
            lightConeMaterial.SetColor(colorName, sourceLight.color * restEdge);
    }

   private Color RandomColor() {
        if(beatColors==null||beatColors.Length==0)return Color.white;
        randomColor = Random.Range(0, beatColors.Length);
        return beatColors[randomColor];
    }

    private IEnumerator ColorShift(Color targetColor) {
        Color currentColor = sourceLight.color;
        Color initialColor = currentColor;
        float timer = 0;
        float edgeValue=restEdge;
        float currentIntensity=sourceLight.intensity;

        while (currentColor != targetColor) {
            currentColor=Color.Lerp(initialColor, targetColor, timer/timeToBeat);
            edgeValue = Mathf.Lerp(edgeValue, beatEdge, timer / timeToBeat);
            currentIntensity= Mathf.Lerp(sourceLight.intensity, beatIntensity, timer / timeToBeat);
            timer += Time.deltaTime;
            sourceLight.color = currentColor;
            sourceLight.intensity = currentIntensity;
            lampMaterial.SetColor(colorName, sourceLight.color * beatIntensity*2);
            if (!noLightCone)
                lightConeMaterial.SetColor(colorName, sourceLight.color * beatEdge);
            yield return null;
        }
        isBeat = false;
    }
}
