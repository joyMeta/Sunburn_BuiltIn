using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour {

    Controls controlMap;
    InputAction moveVectorInputAction;
    InputAction lookVectorInputAction;

    private Vector2 moveVector;
    private Vector2 lookVector;
    private bool sprint;
    private bool jump;
    private bool slide;
    private bool interact;
    private bool interactionMenu;
    private bool sendChat;

    public Vector2 MoveVector => moveVector;
    public Vector2 LookVector => lookVector;
    public bool Sprint => sprint;
    public bool Jump => jump;
    public bool Slide => slide;
    public bool Interact => interact;
    public bool InteractionMenu => interactionMenu;
    public bool SendChat => sendChat;

    private void Awake() {
        controlMap = new Controls();
    }

    private void OnEnable() {
        moveVectorInputAction = controlMap.Player.Move;
        moveVectorInputAction.Enable();
        lookVectorInputAction = controlMap.Player.Look;
        lookVectorInputAction.Enable();
        controlMap.Player.Jump.performed += JumpInput;
        controlMap.Player.Jump.Enable();
        controlMap.Player.Sprint.performed += SprintInput;
        controlMap.Player.Sprint.Enable();
        controlMap.Player.Slide.performed += SlideInput;
        controlMap.Player.Slide.Enable();
        controlMap.Player.Interact.performed += InteractInput;
        controlMap.Player.Interact.Enable();
        controlMap.Player.InteractionMenu.performed += InteractionMenuInput;
        controlMap.Player.InteractionMenu.canceled += InteractionMenuInput;
        controlMap.Player.InteractionMenu.Enable();
        controlMap.Player.SendMessage.performed += SendMessageInput;
        controlMap.Player.SendMessage.canceled += SendMessageInput;
        controlMap.Player.SendMessage.Enable();
    }

    private void JumpInput(InputAction.CallbackContext callbackContext) {
        jump = callbackContext.ReadValueAsButton();
    }

    private void SprintInput(InputAction.CallbackContext callbackContext) {
        sprint = callbackContext.ReadValueAsButton();
    }

    private void SlideInput(InputAction.CallbackContext callbackContext) {
        slide = callbackContext.ReadValueAsButton();
    }

    private void InteractInput(InputAction.CallbackContext callbackContext) {
        interact = callbackContext.ReadValueAsButton();
    }

    private void InteractionMenuInput(InputAction.CallbackContext callbackContext) {
        interactionMenu = callbackContext.ReadValueAsButton();
    }

    private void SendMessageInput(InputAction.CallbackContext callbackContext) {
        if (sendChat) return;
        sendChat = callbackContext.ReadValueAsButton();
    }

    private void Update() {
        moveVector = moveVectorInputAction.ReadValue<Vector2>().normalized;
        lookVector = controlMap.Player.Look.ReadValue<Vector2>();
    }

    private void LateUpdate() {
        if (jump) jump = false;
        if (slide) slide = false;
        if (interact) interact = false;
        if (sendChat) sendChat = false;
    }
}