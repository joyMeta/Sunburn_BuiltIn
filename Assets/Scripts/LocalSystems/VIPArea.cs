using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VIPArea : MonoBehaviour
{
    [SerializeField]
    GameObject vipCodeObject;

    [SerializeField]
    TMP_InputField inputField;

    GameObject cubeObject;

    [SerializeField]
    BoxCollider triggerCollider;
    [SerializeField]
    BoxCollider nonTriggerCollider;

    public void Submit() {
        triggerCollider.enabled = false;
        nonTriggerCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<PhotonView>().IsMine) {
            vipCodeObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        vipCodeObject.SetActive(false);
    }
}
