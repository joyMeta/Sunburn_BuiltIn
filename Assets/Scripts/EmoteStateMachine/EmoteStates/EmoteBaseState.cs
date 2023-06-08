using UnityEngine;

[System.Serializable]
public abstract class EmoteBaseState {

    public bool preUpdateComplete;
    public abstract void EnterState(EmoteStateMachine context, Animator anim, int emoteNumber,Transform syncPosition = null);

    public abstract void PreUpdateState(EmoteStateMachine context);

    public abstract void UpdateState(EmoteStateMachine context);

    public abstract void ExitState(EmoteStateMachine context);
}