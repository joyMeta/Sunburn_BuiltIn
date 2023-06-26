using UnityEngine;
using ReadyPlayerMe.AvatarLoader;
using ReadyPlayerMe.WebView;
using Vuplex.WebView;

public class ReadyPlayerMeWebview : MonoBehaviour {
    private GameObject avatar;
    private AvatarObjectLoader avatarLoader;
    private VuplexWebView vuplexWebView;

    [SerializeField] private GameObject loading;
    [SerializeField] private BaseWebViewPrefab canvasWebView;
    [SerializeField] private UrlConfig urlConfig;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject localPlayerPrefab;

    private void Start() {
        vuplexWebView = new VuplexWebView();
        vuplexWebView.Initialize(canvasWebView, urlConfig);
        vuplexWebView.OnInitialized = () => Debug.Log("WebView Initialized");
        vuplexWebView.OnAvatarUrlReceived = OnAvatarUrlReceived;
        vuplexWebView.OnPageLoadFinished = OnPageLoadFinished;
        vuplexWebView.OnPageLoadStarted = OnPageLoadStarted;
        canvasGroup = canvasWebView.gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    public void OnPageLoadStarted() {
        loading.SetActive(true);
    }

    public void OnPageLoadFinished() {
        loading.SetActive(false);
        canvasGroup.alpha = 1;
    }

    private void OnAvatarUrlReceived(string avatarUrl) {
        PlayerPrefs.SetString("AvatarUrl", avatarUrl);
        loading.SetActive(true);
        avatarLoader = new AvatarObjectLoader();
        avatarLoader.OnCompleted += OnAvatarLoadCompleted;
        avatarLoader.LoadAvatar(avatarUrl);
    }

    private void OnAvatarLoadCompleted(object sender, CompletionEventArgs args) {
        if (avatar != null) {
            Destroy(avatar);
        }
        OnPageLoadStarted();
        FindObjectOfType<PlayerSpawner>().SpawnPlayer();
        avatar = args.Avatar;
        avatar.transform.Rotate(Vector3.up, 180);
        avatar.transform.localPosition = Vector3.zero;

        SetWebViewVisibility(false);
        Debug.Log("Avatar Load Completed");
    }

    public void SetWebViewVisibility(bool visible) {
        canvasWebView.gameObject.SetActive(visible);
    }
}