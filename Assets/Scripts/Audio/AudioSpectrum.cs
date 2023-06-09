using UnityEngine;
using UnityEngine.Rendering;


public enum Channel {
    Left,
    Right,
    Stereo
}
public class AudioSpectrum : MonoBehaviour
{
    private float[] audioSpectrumLeft;
    private float[] audioSpectrumRight;
    [SerializeField]
    public int windowSize = 512;
    [SerializeField]
    public float spectrumScale=10000;
    public AudioSource audioSource;

    [SerializeField]
    int bands = 8;
    public float[] frequencyBands;
    public float[] sensitivity;
    public Channel channel;

    public void Start() {
        audioSpectrumLeft = new float[windowSize];
        audioSpectrumRight = new float[windowSize];
        audioSource=GetComponent<AudioSource>();
        frequencyBands = new float[bands];

    }

    private void Update() {
        GetSpectrumData();
        FrequencyBandSplitting();
    }

    public void GetSpectrumData() {
        audioSource.GetSpectrumData(audioSpectrumLeft, 0, FFTWindow.Blackman);
        audioSource.GetSpectrumData(audioSpectrumRight, 1, FFTWindow.Blackman);
    }

    //void BandBuffer() {
    //    for(int i = 0; i < bands; i++) {
    //        if (frequencyBands[i] > bandBuffer[i]) {
    //            bandBuffer[i] = frequencyBands[i];
    //            bufferDecrease[i] = 0.005f;
    //        }
    //        if(frequencyBands[i] < bandBuffer[i]) {
    //            bandBuffer[i] -= bufferDecrease[i];
    //            bufferDecrease[i] *= 1.2f;
    //        }
    //    }
    //}

    public void FrequencyBandSplitting() {
        /*
         * 20-60
         * 60-250
         * 250-500
         * 500-2000
         * 2000-4000
         * 4000-6000
         * 6000-20000
         * 
         * 0 - 2
         * 1 - 4
         * 2 - 8
         * 3 - 16
         * 4 - 32
         * 5 - 64
         * 6 - 128
         * 7 - 256
         * */

        switch (channel) {
            case Channel.Left: {
                    int count = 0;
                    for (int i = 0; i < bands; i++) {
                        float average = 0;
                        int sampleCount = (int)Mathf.Pow(2, i + 1);
                        if (i == 7)
                            sampleCount += 2;
                        for (int j = 0; j < sampleCount; j++) {
                            average += audioSpectrumLeft[count] * (count + 1);
                            count++;
                        }
                        average /= count;
                        frequencyBands[i] = average * spectrumScale;
                    }
                    break;
                }
                case Channel.Right: {
                    int count = 0;
                    for (int i = 0; i < bands; i++) {
                        float average = 0;
                        int sampleCount = (int)Mathf.Pow(2, i + 1);
                        if (i == 7)
                            sampleCount += 2;
                        for (int j = 0; j < sampleCount; j++) {
                            average += audioSpectrumRight[count] * (count + 1);
                            count++;
                        }
                        average /= count;
                        frequencyBands[i] = average * spectrumScale;
                    }
                    break;
                }
            case Channel.Stereo: {
                    int count = 0;
                    for (int i = 0; i < bands; i++) {
                        float average = 0;
                        int sampleCount = (int)Mathf.Pow(2, i + 1);
                        if (i == 7)
                            sampleCount += 2;
                        for (int j = 0; j < sampleCount; j++) {
                            average += (audioSpectrumLeft[count] + audioSpectrumRight[count]) * (count + 1);
                            count++;
                        }
                        average /= count;
                        frequencyBands[i] = average * spectrumScale;
                    }
                    break;
                }
        }
       
    }
}
