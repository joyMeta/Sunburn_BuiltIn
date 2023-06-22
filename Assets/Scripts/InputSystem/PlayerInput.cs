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
    private bool interactionMenu;
    private bool dance_1;
    private bool dance_2;
    private bool dance_3;
    private bool dance_4;
    private bool dance_5;
    private bool emote_1;
    private bool emote_2;
    private bool emote_3;

    public Vector2 MoveVector => moveVector;
    public Vector2 LookVector => lookVector;
    public bool Sprint => sprint;
    public bool Jump => jump;
    public bool Slide => slide;
    public bool InteractionMenu => interactionMenu;
    public bool Dance_1 => dance_1;
    public bool Dance_2 => dance_2;
    public bool Dance_3 => dance_3;
    public bool Dance_4 => dance_4;
    public bool Dance_5 => dance_5;
    public bool Emote_1 => emote_1;
    public bool Emote_2 => emote_2;
    public bool Emote_3 => emote_3;

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
        controlMap.Player.InteractionMenu.performed += InteractionMenuInput;
        controlMap.Player.InteractionMenu.Enable();
        controlMap.Player.Dance1.performed += DanceInput_1;
        controlMap.Player.Dance1.Enable();
        controlMap.Player.Dance2.performed += DanceInput_2;
        controlMap.Player.Dance2.Enable();
        controlMap.Player.Dance3.performed += DanceInput_3;
        controlMap.Player.Dance3.Enable();
        controlMap.Player.Dance4.performed += DanceInput_4;
        controlMap.Player.Dance4.Enable();
        controlMap.Player.Dance5.performed += DanceInput_5;
        controlMap.Player.Dance5.Enable();
        controlMap.Player.Emote1.performed += EmoteInput_1;
        controlMap.Player.Emote1.Enable();
        controlMap.Player.Emote2.performed += EmoteInput_2;
        controlMap.Player.Emote2.Enable();
        controlMap.Player.Emote3.performed += EmoteInput_3;
        controlMap.Player.Emote3.Enable();
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

    private void InteractionMenuInput(InputAction.CallbackContext callbackContext) {
        interactionMenu = !interactionMenu;
    }

    private void DanceInput_1(InputAction.CallbackContext callbackContext) {
        dance_1 = callbackContext.ReadValueAsButton();
    }
    private void DanceInput_2(InputAction.CallbackContext callbackContext) {
        dance_2 = callbackContext.ReadValueAsButton();
    }
    private void DanceInput_3(InputAction.CallbackContext callbackContext) {
        dance_3 = callbackContext.ReadValueAsButton();
    }
    private void DanceInput_4(InputAction.CallbackContext callbackContext) {
        dance_4 = callbackContext.ReadValueAsButton();
    }
    private void DanceInput_5(InputAction.CallbackContext callbackContext) {
        dance_5 = callbackContext.ReadValueAsButton();
    }
    private void EmoteInput_1(InputAction.CallbackContext callbackContext) {
        emote_1 = callbackContext.ReadValueAsButton();
    }
    private void EmoteInput_2(InputAction.CallbackContext callbackContext) {
        emote_2 = callbackContext.ReadValueAsButton();
    }
    private void EmoteInput_3(InputAction.CallbackContext callbackContext) {
        emote_3 = callbackContext.ReadValueAsButton();
    }

    private void Update() {
        moveVector = moveVectorInputAction.ReadValue<Vector2>().normalized;
        lookVector = controlMap.Player.Look.ReadValue<Vector2>();
    }

    private void LateUpdate() {
        if (jump) jump = false;
        if (slide) slide = false;
        if (dance_1) dance_1 = false;
        if (dance_2) dance_2 = false;
        if (dance_3) dance_3 = false;
        if (dance_4) dance_4 = false;
        if (dance_5) dance_5 = false;
        if(emote_1) emote_1 = false;
        if(emote_2) emote_2 = false;
        if(emote_3) emote_3 = false;
    }
}