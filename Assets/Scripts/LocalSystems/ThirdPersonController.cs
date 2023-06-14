using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

public class ThirdPersonController : MonoBehaviour {
    [Header("Player")]
    public float forwardJumpForce;
    public float verticalJumpForce;
    [Tooltip("How fast the character turns to face movement direction")]
    public float rotationVelocity;
    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    [Space(10)]
    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;

    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;

    [Tooltip("Start of ground check raycast")]
    [SerializeField]
    private Vector3 sensorOffset;

    [Tooltip("Length of ground check raycast")]
    [SerializeField]
    private float sensorLength;
    [SerializeField]
    private float moveSpeedMultiplier = 1;

    private Rigidbody rb;

    private float speed;
    private float turnAmount;

    public float Speed => speed;
    public float TurnAmount => turnAmount;

    [SerializeField]
    GameObject depthCheck;
    [SerializeField]
    float stepHeight = 0.3f;


    private PlayerInput input;
    public PlayerInput Input => input;
    private GameObject mainCamera;

    private Vector3 cameraRelativeMovement;

    public Vector3 CameraRelativeMovement => cameraRelativeMovement;
    public float gravityMultiplier = 1;

    private Vector3 tempForwardVelocity = Vector3.zero;
    private bool hasAnimator;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    AudioSource footStepAudioSource;

    protected void Start() {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();
    }

    protected void Update() {
        animator.applyRootMotion = animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle");
        GroundedCheck();
        Move();
    }

    protected void FixedUpdate() {
        Vector3 extraGravityForce = new Vector3(0, 0, 0);
        extraGravityForce += new Vector3(0, Physics.gravity.y*gravityMultiplier, tempForwardVelocity.z);
        rb.AddForce(extraGravityForce);
    }

    private void GroundedCheck() {
        RaycastHit depthHit;
        Debug.DrawRay(depthCheck.transform.position, depthCheck.transform.forward* (depthCheck.transform.localPosition.y + stepHeight), Color.red);
        if (Physics.Raycast(depthCheck.transform.position, depthCheck.transform.forward, out depthHit, depthCheck.transform.localPosition.y + stepHeight)) {
            float difference = Mathf.Abs(depthHit.point.y - transform.position.y);
            if (difference>0&&difference<stepHeight) {
                Vector3 newPosition = new Vector3(transform.position.x, depthHit.point.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime *2);
            }
            Grounded = depthHit.transform!=null;
        }
        else {
            tempForwardVelocity = transform.forward * forwardJumpForce * speed;
            RaycastHit hit;
            Vector3 sensorPosition = transform.position;
            sensorPosition += transform.forward * sensorOffset.z;
            sensorPosition += transform.up * sensorOffset.y;
            Physics.Raycast(sensorPosition + sensorOffset, -transform.up, out hit, stepHeight, GroundLayers);
            Debug.DrawRay(sensorPosition + sensorOffset, -transform.up * stepHeight, Color.red);
            Grounded = hit.transform != null;
        }
    }

    public void Jump() {
        if (!Grounded)
            return;
        tempForwardVelocity = transform.forward * forwardJumpForce * speed;
        Vector3 jumpVector = transform.up * verticalJumpForce + transform.forward * forwardJumpForce * input.MoveVector.y;
        rb.velocity = jumpVector;
    }

    public void DisableRootMotion() {
        animator.applyRootMotion = false;
    }

    private void Move() {
        speed = input.MoveVector.magnitude;
        cameraRelativeMovement = input.MoveVector.y * mainCamera.transform.forward + input.MoveVector.x * mainCamera.transform.right;
        cameraRelativeMovement.y = 0;
        cameraRelativeMovement.Normalize();
        Debug.DrawRay(transform.position, cameraRelativeMovement, Color.blue);
        turnAmount = Vector3.SignedAngle(transform.forward, cameraRelativeMovement, transform.up);
        if (cameraRelativeMovement.magnitude > 0)
            transform.Rotate(0, turnAmount * Time.deltaTime * rotationVelocity, 0);
    }

    private void OnAnimatorMove() {
        if (animator == null || rb == null)
            return;
        if (Grounded && Time.deltaTime > 0) {
            Vector3 v = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;
            rb.velocity = v;
        }
    }

    public void PlayFootstep() {
        footStepAudioSource.PlayOneShot(FootstepAudioClips[Random.Range(0, FootstepAudioClips.Length - 1)], 1);
    }

    public void PlayLandingSound() {
        footStepAudioSource.PlayOneShot(LandingAudioClip, 1);
    }
}