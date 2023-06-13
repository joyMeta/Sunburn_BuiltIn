using System;
using Photon.Voice;
using Photon.Voice.Unity;
using UnityEngine;
using ILogger = Photon.Voice.ILogger;

public class AudioSourceInputFactorySetter : VoiceComponent {
    private AudioSourceInputFactory audioSourcePusher;

    [SerializeField]
    private Recorder recorder;

    [SerializeField]
    private AudioSource audioSource;

    protected override void Awake() {
        audioSourcePusher = new AudioSourceInputFactory(audioSource, Logger);
        recorder.SourceType = Recorder.InputSourceType.Factory;
        recorder.InputFactory = InputFactory;
    }

    private IAudioDesc InputFactory() {
        return audioSourcePusher;
    }
}

public class AudioSourceInputFactory : IAudioPusher<float> {

    private AudioSource audioSource;
    private ILogger logger;

    private AudioOutCapture audioOutCapture;

    private int sampleRate;
    private int channels;

    public int SamplingRate { get { return Error == null ? sampleRate : 0; } }
    public int Channels { get { return Error == null ? channels : 0; } }
    public string Error { get; private set; }

    public AudioSourceInputFactory(AudioSource aS, ILogger lg) {
        try {
            logger = lg;
            audioSource = aS;
            sampleRate = AudioSettings.outputSampleRate;
            switch (AudioSettings.speakerMode) {
                case AudioSpeakerMode.Mono: channels = 1; break;
                case AudioSpeakerMode.Stereo: channels = 2; break;
                default:
                    Error = string.Concat("Only Mono and Stereo project speaker mode supported. Current mode is ", AudioSettings.speakerMode);
                    logger.LogError("AudioSourceInputFactory: {0}", Error);
                    return;
            }
            if (!audioSource.enabled) {
                logger.LogWarning("AudioSourceInputFactory: AudioSource component disabled, enabling it.");
                audioSource.enabled = true;
            }
            if (!audioSource.gameObject.activeSelf) {
                logger.LogWarning("AudioSourceInputFactory: AudioSource GameObject inactive, activating it.");
                audioSource.gameObject.SetActive(true);
            }
            if (!audioSource.gameObject.activeInHierarchy) {
                Error = "AudioSource GameObject is not active in hierarchy, audio input can't work.";
                logger.LogError("AudioSourceInputFactory: {0}", Error);
                return;
            }
            audioOutCapture = audioSource.gameObject.GetComponent<AudioOutCapture>();
            if (ReferenceEquals(null, audioOutCapture) || !audioOutCapture) {
                audioOutCapture = audioSource.gameObject.AddComponent<AudioOutCapture>();
            }
            if (!audioOutCapture.enabled) {
                logger.LogWarning("AudioSourceInputFactory: AudioOutCapture component disabled, enabling it.");
                audioOutCapture.enabled = true;
            }
        }
        catch (Exception e) {
            Error = e.ToString();
            if (Error == null) // should never happen but since Error used as validity flag, make sure that it's not null
            {
                Error = "Exception in MicWrapperPusher constructor";
            }
            logger.LogError("AudioSourceInputFactory: {0}", Error);
        }
    }

    private float[] frame2 = Array.Empty<float>();

    private void AudioOutCaptureOnOnAudioFrame(float[] frame, int channelsNumber) {
        if (channelsNumber != Channels) {
            logger.LogWarning("AudioSourceInputFactory: channels number mismatch; expected:{0} got:{1}.", Channels, channelsNumber);
        }
        if (frame2.Length != frame.Length) {
            frame2 = new float[frame.Length];
        }
        Array.Copy(frame, frame2, frame.Length);
        pushCallback(frame);
        Array.Clear(frame, 0, frame.Length);
    }

    private Action<float[]> pushCallback;

    public void SetCallback(Action<float[]> callback, ObjectFactory<float[], int> bufferFactory) {
        pushCallback = callback;
        audioOutCapture.OnAudioFrame += AudioOutCaptureOnOnAudioFrame;
    }

    public void Dispose() {
        if (pushCallback != null && audioOutCapture != null) {
            audioOutCapture.OnAudioFrame -= AudioOutCaptureOnOnAudioFrame;
        }
    }
}