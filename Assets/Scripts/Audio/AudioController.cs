using Photon.Compression;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

    public Slider[] sliders;
    private void Start() {
        audioSource= GetComponent<AudioSource>();
        audioSpectrum = FindObjectOfType<AudioSpectrum>();
        for(int i=0;i<audioSpectrum.sensitivity.Length;i++) {
            sliders[i].value= audioSpectrum.sensitivity[i];
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
                go.GetComponent<AudioSetup>().SetupAudio( clip.name);
                
            }
        }
        yield return new WaitForEndOfFrame();
    }

    //public async Task<AudioClip> LoadFile(string path) {
    //    AudioClip clip = null;
    //    using(UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(path,AudioType.MPEG)) {
    //        request.SendWebRequest();
    //        try {
    //            while (!request.isDone) await Task.Delay(5);
    //            if (request.result!=UnityWebRequest.Result.Success)  
    //                Debug.Log($"{request.error}");
    //            else
    //                clip = DownloadHandlerAudioClip.GetContent(request);
    //        }
    //        catch (Exception err) {
    //            Debug.Log($"{err.Message}, {err.StackTrace}");
    //        }
    //    }
    //    return clip;
    //}

    public void SetSensitivity(int index) {
        audioSpectrum.sensitivity[index] = sliders[index].value;
    }

    public void PlayTrack(string name) {
        if (name == currentTrack)
            audioSource.Pause();
        else {
            audioSource.clip = audioClips[name];
            audioSource.Play();
        }
    }
}