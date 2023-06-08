using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public static class CSVDownloader {
    private const string startString = "https://docs.google.com/spreadsheets/d/";
    private const string endString = "/export?format=csv";

    internal static IEnumerator DownloadData(string link, System.Action<string> onCompleted) {

        link=startString+link+endString;
        yield return new WaitForEndOfFrame();
        string downloadData = null;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(link)) {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError) {
                Debug.LogError("...Download Error: " + webRequest.error);
            }
            else {
                downloadData = webRequest.downloadHandler.text;
                onCompleted(downloadData);
            }
        }
    }
}