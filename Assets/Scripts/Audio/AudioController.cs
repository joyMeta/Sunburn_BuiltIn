using Photon.Pun;
using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AudioController : MonoBehaviour {
    AudioSpectrum audioSpectrum;
    AudioSource audioSource;
    public Transform audioTracksParent;
    public GameObject audioTrackPrefab;
    [SerializeField]
    Dictionary<string, AudioClip> audioClips=new Dictionary<string, AudioClip>();
    string currentTrack="";
    [SerializeField]
    Slider musicSeekBar;
    [SerializeField]
    Animator seekAnimator;
    [SerializeField]
    TMP_Text musicName;

    [SerializeField]
    public Slider[] sliders;
    Recorder recorder;

    [SerializeField]
    Color enabledColor;
    [SerializeField]
    Color disabledColor;

    [SerializeField]
    Image playImage;
    [SerializeField]
    Image pauseImage;


    [SerializeField]
    AudioMixer audioMixer;
    [SerializeField]
    Toggle muteToggle;

    private void Awake() {
        recorder=GetComponent<Recorder>();
        audioSource = GetComponentInChildren<AudioSource>();
        audioSpectrum = GetComponentInParent<AudioSpectrum>();
        for (int i = 0; i < audioSpectrum.sensitivity.Length; i++) {
            sliders[i].value = audioSpectrum.sensitivity[i];
        }
        if (Utilities.IntBoolConverter.IntToBool(PlayerPrefs.GetInt("MasterPlayer"))) {
            string dataPath = Path.Combine(Application.streamingAssetsPath);
            List<string> paths = Directory.GetFiles(dataPath, "*.mp3").ToList();
            foreach (string path in paths)
                StartCoroutine(LoadAudioFile(path));
        }
    }

    public IEnumerator LoadAudioFile(string path) {
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG)) {
            yield return request.SendWebRequest();
            while(!request.isDone)
                yield return null;
            if (request.result == UnityWebRequest.Result.Success) {
                string[] name = path.Split("StreamingAssets");
                name[1] = name[1].Replace(@"\", string.Empty);
                AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
                clip.name = name[1];
                audioClips.Add(name[1],clip);
                GameObject go = Instantiate(audioTrackPrefab, audioTracksParent);
                go.GetComponent<AudioSetup>().SetupAudio(clip.name);
            }
        }
        yield return new WaitForEndOfFrame();
    }

    public void SetSensitivity(int index) {
        audioSpectrum.sensitivity[index] = sliders[index].value;
    }

    public void PlayTrack(string name) {
        StopCoroutine(DelayHide());
        musicName.text = name;
        if (name == currentTrack) {
            if (audioSource.isPlaying) { 
                audioSource.Pause();
            }
            else { 
                audioSource.Play();
            }
        }
        else {
            audioSource.clip = audioClips[name];
            currentTrack = audioSource.clip.name;
            audioSource.Play();
        }
        if (recorder != null)
            recorder.AudioClip = audioSource.clip;
        StartCoroutine(DelayHide());
    }

    private void LateUpdate() {
        if (muteToggle.isOn) {
            audioMixer.SetFloat("Music", -20f);
        }
        else {
            audioMixer.SetFloat("Music", -100f);
        }
        if (audioSource.isPlaying) {
            playImage.color=Color.Lerp(playImage.color,enabledColor, Time.deltaTime*5);
            pauseImage.color=Color.Lerp(pauseImage.color,disabledColor, Time.deltaTime*5);
        }
        else {
            pauseImage.color = Color.Lerp(pauseImage.color, enabledColor, Time.deltaTime*5);
            playImage.color = Color.Lerp(playImage.color, disabledColor, Time.deltaTime * 5);
        }
        if(audioSource.clip != null)
            musicSeekBar.value = audioSource.time/audioSource.clip.length;
    }

    IEnumerator DelayHide() {
        seekAnimator.SetBool("Open", true);
        yield return new WaitForSeconds(2);
        seekAnimator.SetBool("Open", false);
    }
}