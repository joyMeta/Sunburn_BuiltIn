using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera mainCamera;

    public void Start() {
        mainCamera=Camera.main;
    }

    public void LateUpdate() {
        if (mainCamera != null) {
            transform.LookAt(mainCamera.transform.position);
        }
    }

    public void DeleteGameObject() {
        Destroy(gameObject);
    }
}
