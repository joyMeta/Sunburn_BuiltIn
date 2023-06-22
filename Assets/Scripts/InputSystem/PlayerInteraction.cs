using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour {
    Controls controlMap;
    private bool interact;
    private bool interactionMenu;
    private bool sendChat;
    private bool chat;

    public bool Interact => interact;
    public bool InteractionMenu => interactionMenu;
    public bool SendChat => sendChat;
    public bool Chat => chat;

    void Awake() {
        controlMap = new Controls();
    }

    private void OnEnable() {
        controlMap.Player.Interact.performed += InteractInput;
        controlMap.Player.Interact.Enable();
        controlMap.Player.InteractionMenu.performed += InteractionMenuInput;
        controlMap.Player.InteractionMenu.Enable();
        controlMap.Player.Chat.performed += ChatInput;
        controlMap.Player.Chat.Enable();
        controlMap.Player.SendMessage.performed += SendMessageInput;
        controlMap.Player.SendMessage.canceled += SendMessageInput;
        controlMap.Player.SendMessage.Enable();
    }

    private void InteractInput(InputAction.CallbackContext callbackContext) {
        interact = callbackContext.ReadValueAsButton();
    }

    public void ChatInput(InputAction.CallbackContext callbackContext) {
        chat = !chat;// callbackContext.ReadValueAsButton();
    }

    private void InteractionMenuInput(InputAction.CallbackContext callbackContext) {
        interactionMenu = !interactionMenu;
    }

    private void SendMessageInput(InputAction.CallbackContext callbackContext) {
      sendChat=!sendChat;
    }

    public void LateUpdate() {
        if (interact) interact = false;
    }
}