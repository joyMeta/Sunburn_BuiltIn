using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[System.Serializable]
public class EmoteHandshake : EmoteBaseState {
    [SerializeField]
    float yOffset=1.2f;
    public Transform handShakeTargetTransform;
    [SerializeField]
    float amplitude;
    [SerializeField]
    float frequency;
    Animator _anim;
    [SerializeField]
    Rig handShakeRig;
    [SerializeField]
    AnimationClip handShakeIdle;
    float timeElapsed;
    Transform _syncPosition;
    public Transform SyncPosition=>_syncPosition;

    public override void EnterState(EmoteStateMachine context, Animator anim, int emoteNumber, Transform syncPosition) {
        _anim = anim;
        timeElapsed = 0;
        if (!context.instigator)
            Debug.Log(syncPosition.position);
        _syncPosition = syncPosition;
        handShakeTargetTransform.position = new Vector3(handShakeTargetTransform.position.x, yOffset, handShakeTargetTransform.position.z);
    }

    public override void PreUpdateState(EmoteStateMachine context) {
        handShakeRig.weight = Mathf.Lerp(handShakeRig.weight, 1, Time.deltaTime * context.LerpSpeed);
        preUpdateComplete = (handShakeRig.weight >= 0.98f);
        if (preUpdateComplete) {
            _anim.SetTrigger("Emote");
            _anim.SetInteger("EmoteType", 1);
        }
    }

    public override void UpdateState(EmoteStateMachine context) {
        if (!context.instigator) {
            context.transform.position = Vector3.Lerp(context.transform.position, _syncPosition.position, Time.deltaTime * context.LerpSpeed);
            context.transform.rotation = Quaternion.Slerp(context.transform.rotation, _syncPosition.rotation, Time.deltaTime * context.LerpSpeed);
            handShakeTargetTransform.position = context.requestedPlayer.emoteStates.emoteHandshake.handShakeTargetTransform.position;
        }
        if (context.instigator) {
            float yPosition = amplitude * Mathf.Sin(Time.time * frequency);
            handShakeTargetTransform.position += new Vector3(0, yPosition, 0);
        }
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= handShakeIdle.length / 2 + 0.4f) {
            handShakeRig.weight = 0;
            ExitState(context);
        }
    }

    public override void ExitState(EmoteStateMachine context) {
        context.instigator = false;
        preUpdateComplete = false;
        context.PlayEmote(0);
    }
}