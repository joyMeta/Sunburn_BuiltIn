using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacter : MonoBehaviour
{
    Animator _animator;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    public void PlayEmote(int index) {
        switch (index) {
            case 0: {

                    break;
                }
            case 1: {
                    _animator.SetInteger("EmoteType",index);
                    break;
                }
        }
    }
}
