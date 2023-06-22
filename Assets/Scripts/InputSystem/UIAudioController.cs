using UnityEngine;
using UnityEngine.InputSystem;

public class UIAudioController : MonoBehaviour
{
    Controls controlMap;

    private bool mixer;
    private bool playlist;

    public bool Mixer=>mixer;
    public bool Playlist=>playlist;

    public Animator mixerAnimator;
    public Animator playlistAnimator;
    bool masterPlayer;

    void Awake() {
        controlMap = new Controls();
        masterPlayer = Utilities.IntBoolConverter.IntToBool(PlayerPrefs.GetInt("MasterPlayer"));
    }

    private void OnEnable() {
        controlMap.Player.Mixer.performed += MixerInput;
        controlMap.Player.Mixer.Enable();
        controlMap.Player.Playlist.performed += PlaylistInput;
        controlMap.Player.Playlist.Enable();
    }

    private void MixerInput(InputAction.CallbackContext callbackContext) {
        mixer = !mixer;
        playlist = false;
    }

    public void PlaylistInput(InputAction.CallbackContext callbackContext) {
        playlist = !playlist;
        mixer = false;
    }

    private void Update() {
        mixerAnimator.SetBool("Open", mixer);
        if (!masterPlayer)
            return;
        playlistAnimator.SetBool("Open", playlist);
    }
}
