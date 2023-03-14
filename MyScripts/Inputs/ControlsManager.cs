using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControlsManager : MonoBehaviour
{
    public PlayerControls Inputs { get; private set; }
    public PlayerInput PlayerInput { get; private set; }

    #region InputActions
    public InputAction StickPosition { get; private set; }
    InputAction mousePosition;
    public InputAction Dash { get; private set; }
    public InputAction Movement { get; private set; }
    public InputAction Shoot { get; private set; }
    public InputAction UsePowerup { get; private set; }
    public InputAction DiscardPowerup { get; private set; }
    public InputAction EgoBoost { get; private set; }
    public InputAction PauseButton { get; private set; }
    #endregion InputActions

    Crosshair crosshair;

    #region Device Checking

    public delegate void MouseAsDevice();
    public MouseAsDevice UseMouse;

    public delegate void GamepadAsDevice();
    public GamepadAsDevice UseGamepad;

    bool usingGamepad;
    public bool GamepadInUse
    {
        get { return usingGamepad; }
        set
        {
            if (usingGamepad == false && value == true) UseGamepad?.Invoke();
            if (usingGamepad == true && value == false) UseMouse?.Invoke();
            usingGamepad = value;
        }
    }
    #endregion
    

    void Awake()
    {
        crosshair = FindObjectOfType<Crosshair>();
        this.PlayerInput = GetComponent<PlayerInput>();
        Inputs = new PlayerControls();

        StickPosition = this.PlayerInput.actions["StickPosition"];
        mousePosition = this.PlayerInput.actions["MousePosition"];
        Dash = this.PlayerInput.actions["Dash"];
        Movement = this.PlayerInput.actions["Movement"];
        Shoot = this.PlayerInput.actions["Shoot"];        
        UsePowerup = this.PlayerInput.actions["Powerup"];
        DiscardPowerup = this.PlayerInput.actions["DiscardPowerup"];
        EgoBoost = this.PlayerInput.actions["EgoBoost"];
        PauseButton = this.PlayerInput.actions["Pause"];
        
        Inputs.DeviceCheck.MouseUsed.performed += _ => { GamepadInUse = false; crosshair.EnableCrosshair(); };
        Inputs.DeviceCheck.KeyboardUsed.performed += _ => { GamepadInUse = false; crosshair.EnableCrosshair(); };
        Inputs.DeviceCheck.GamepadUsed.performed += _ => { GamepadInUse = true; crosshair.DisableCrosshair(); };
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

    private void OnEnable()
    {
        this.PlayerInput.enabled = true;
        Inputs.Enable();
        Inputs.DeviceCheck.Enable();
        EnablePlayerControls();
    }
    private void OnDisable()
    {
        this.PlayerInput.enabled = false;
        GamepadInUse = false;
        Inputs.Disable();
        Inputs.DeviceCheck.Disable();
        Inputs.UI.Disable();
    }
    public void EnablePlayerControls()
    {
        this.PlayerInput.enabled = true;
        Inputs.UI.Disable();
    }

    public void DisablePlayerControls()
    {
        this.PlayerInput.enabled = false;
        Inputs.UI.Enable();
    }

    public Vector2 GetAimPosition()
    {
        Vector2 aimPos;
        if (!GamepadInUse && this.PlayerInput.enabled) aimPos = Camera.main.ScreenToWorldPoint(mousePosition.ReadValue<Vector2>());
        else if (GamepadInUse && this.PlayerInput.enabled) aimPos = StickPosition.ReadValue<Vector2>();
        else aimPos = Camera.main.ScreenToWorldPoint(Inputs.UI.Point.ReadValue<Vector2>());

        return aimPos;
    }
}
