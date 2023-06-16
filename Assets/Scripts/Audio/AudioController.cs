using Photon.Pun;
using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
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
    TMP_Text musicName;

    [SerializeField]
    public Slider[] sliders;
    NetworkAudioManager networkAudioManager;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        audioSpectrum = FindObjectOfType<AudioSpectrum>();
        networkAudioManager=GetComponent<NetworkAudioManager>();
        for (int i = 0; i < audioSpectrum.sensitivity.Length; i++) {
            sliders[i].value = audioSpectrum.sensitivity[i];
        }
        string dataPath = Path.Combine(Application.streamingAssetsPath);
        List<string> paths = Directory.GetFiles(dataPath, "*.mp3").ToList();
        foreach (string path in paths)
            StartCoroutine(LoadAudioFile(path));
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
        musicName.text = name;
        if (name == currentTrack) {
            if (audioSource.isPlaying) { 
                audioSource.Pause();
                if (networkAudioManager != null)
                    networkAudioManager.PlayAudio(audioSource.clip, false);
            }
            else { 
                audioSource.Play();
                if (networkAudioManager != null)
                    networkAudioManager.PlayAudio(audioSource.clip, true);
            }
        }
        else {
            audioSource.clip = audioClips[name];
            currentTrack = audioSource.clip.name;
            audioSource.Play();
            if (networkAudioManager != null)
                networkAudioManager.PlayAudio(audioSource.clip, true);
        }
    }

    private void LateUpdate() {
        if(audioSource.clip != null)
            musicSeekBar.value = audioSource.time/audioSource.clip.length;
    }
}