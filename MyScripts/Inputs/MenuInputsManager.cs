using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MenuInputsManager : MonoBehaviour
{
    PlayerControls controls;
    [SerializeField] GameObject selectedButton;
    #region Gamepad Bools

    bool UsingGamepad;
    public bool GamepadInUse
    {
        get { return UsingGamepad; }
        set
        {
            if (UsingGamepad == false && value == true) EventSystem.current.SetSelectedGameObject(selectedButton);
            if (UsingGamepad == true && value == false) EventSystem.current.SetSelectedGameObject(null);
            UsingGamepad = value;
        }
    }
    #endregion

    private void Awake()
    {
        controls = new PlayerControls();
        controls.DeviceCheck.MouseUsed.performed += _ => GamepadInUse = false;
        controls.DeviceCheck.KeyboardUsed.performed += _ => GamepadInUse = false;
        controls.DeviceCheck.GamepadUsed.performed += _ => GamepadInUse = true;
    }

    private void OnEnable()
    {
        if (Gamepad.current != null)
        {
            GamepadInUse = true;
            EventSystem.current.firstSelectedGameObject = selectedButton;
        }
        controls.Enable();
        controls.UI.Enable();
        controls.DeviceCheck.Enable();
    }
    private void OnDisable()
    {
        GamepadInUse = false;
        controls.Disable();
        controls.UI.Disable();
        controls.DeviceCheck.Disable();
    }
    private void Start()
    {
        InputSystem.onDeviceChange += (device, change) =>
        {
            switch (change)
            {
                case InputDeviceChange.Added:
                    GamepadInUse = true;
                    break;
                case InputDeviceChange.Disconnected:
                    GamepadInUse = false;
                    break;
                case InputDeviceChange.Reconnected:
                    GamepadInUse = true;
                    break;
                case InputDeviceChange.Removed:
                    GamepadInUse = false;
                    break;
                default:
                    GamepadInUse = false;
                    break;
            }
        };
    }
}
