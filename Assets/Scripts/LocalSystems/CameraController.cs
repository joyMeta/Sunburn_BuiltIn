using UnityEngine;

public class CameraController : MonoBehaviour {
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;
    private PlayerInput input;
    private const float threshold = 0.01f;
    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;
    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;
    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;
    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;
    [Tooltip("Cinemachine look target")]
    public GameObject CinemachineCameraTarget;
    [Tooltip("Responsiveness of the camera")]
    public Vector2 cameraMovementSpeed = new Vector2(5, 3);


    void Start() {
        cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
        input = GetComponent<PlayerInput>();
        //Cursor.visible= false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void Update() {
        if (input.InteractionMenu) {
            Cursor.visible = !Cursor.visible;
            if (Cursor.lockState == CursorLockMode.None)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }
    }

    private void LateUpdate() {
        CameraRotation();
    }

    private void CameraRotation() {
        if (input.LookVector.sqrMagnitude >= threshold && !LockCameraPosition) {
            cinemachineTargetYaw += input.LookVector.x * Time.deltaTime*cameraMovementSpeed.y*10;
            cinemachineTargetPitch += input.LookVector.y * Time.deltaTime*cameraMovementSpeed.x*10;
        }

        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, BottomClamp, TopClamp);

        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + CameraAngleOverride,
            cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax) {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}