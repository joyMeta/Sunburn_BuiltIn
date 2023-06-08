using UnityEngine;

[System.Serializable]
public class EmoteStates {
    public EmoteIdle emoteIdle;
    public EmoteHandshake emoteHandshake;
}

public class EmoteStateMachine : MonoBehaviour {
    Animator _animator;
    EmoteBaseState _currentState;
    PlayerInput _playerInput;
    public EmoteBaseState CurrentState => _currentState;
    public EmoteStates emoteStates = new();
    public EmoteStateMachine requestedPlayer;

    public bool instigator = false;

    ThirdPersonController _thirdPersonController;

    [SerializeField]
    private float _lerpSpeed = 1;

    public float LerpSpeed => _lerpSpeed;

    public void Awake() {
        _currentState = emoteStates.emoteIdle;
        _animator = GetComponent<Animator>();
        _thirdPersonController = GetComponent<ThirdPersonController>();
        _playerInput=GetComponent<PlayerInput>();
    }

    public void Update() {
        if (!_currentState.preUpdateComplete) {
            _currentState.PreUpdateState(this);
            return;
        }
        _currentState.UpdateState(this);
    }

    public void PlayEmote(int emoteIndex,Transform syncPosition=null) {
        Debug.Log("Set Emote");
        switch (emoteIndex) {
            case 0: {
                    _thirdPersonController.enabled = true;
                    _currentState = emoteStates.emoteIdle;
                    break;
                }
            case 1: {
                    _currentState = emoteStates.emoteHandshake;
                    break;
                }

        }
        _currentState.EnterState(this, _animator, emoteIndex, syncPosition);
    }

    public void ResponseEmotePlay(int emoteIndex) {
        switch (emoteIndex) {
            case 0: {
                    _currentState = emoteStates.emoteIdle;
                    break;
                }
            case 1: {
                    _currentState = emoteStates.emoteHandshake;
                    break;
                }

        }
        _currentState.EnterState(this, _animator, emoteIndex);
    }
}