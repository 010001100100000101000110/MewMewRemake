using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MenuInputsManager : MonoBehaviour
{
    PlayerControls controls;
    DeviceChecking deviceCheck;
    [SerializeField] GameObject firstSelectedButton;

    private void Awake()
    {
        controls = new PlayerControls();
        deviceCheck = FindObjectOfType<DeviceChecking>();
        controls.DeviceCheck.MouseUsed.performed += _ => deviceCheck.GamepadInUse = false;
        controls.DeviceCheck.KeyboardUsed.performed += _ => deviceCheck.GamepadInUse = false;
        controls.DeviceCheck.GamepadUsed.performed += _ => deviceCheck.GamepadInUse = true;
    }

    private void OnEnable()
    {
        if (Gamepad.current != null)
        {
            deviceCheck.GamepadInUse = true;
            EventSystem.current.firstSelectedGameObject = firstSelectedButton;
        }
        controls.Enable();
        controls.UI.Enable();
        controls.DeviceCheck.Enable();
    }
    private void OnDisable()
    {
        deviceCheck.GamepadInUse = false;
        controls.Disable();
        controls.UI.Disable();
        controls.DeviceCheck.Disable();
    }
}
