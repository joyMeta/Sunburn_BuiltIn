using UnityEngine;

public class AudioSpectrum : MonoBehaviour
{
    private float[] audioSpectrum;
    public static float spectrumValue { get;private set; }
    public AudioSource audioSource;

    public void Start() {
        audioSpectrum = new float[128];
        audioSource=GetComponent<AudioSource>();
    }

    private void Update() {
        audioSource.GetOutputData(audioSpectrum, 0);
        if (audioSpectrum != null && audioSpectrum.Length> 0) {
            spectrumValue= audioSpectrum[0]*100;
        }
    }
}
