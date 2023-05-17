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

    public DeviceChecking DeviceCheck { get; private set; }

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

    void Awake()
    {
        crosshair = FindObjectOfType<Crosshair>();
        this.PlayerInput = GetComponent<PlayerInput>();
        Inputs = new PlayerControls();
        DeviceCheck = FindObjectOfType<DeviceChecking>();

        StickPosition = this.PlayerInput.actions["StickPosition"];
        mousePosition = this.PlayerInput.actions["MousePosition"];
        Dash = this.PlayerInput.actions["Dash"];
        Movement = this.PlayerInput.actions["Movement"];
        Shoot = this.PlayerInput.actions["Shoot"];        
        UsePowerup = this.PlayerInput.actions["Powerup"];
        DiscardPowerup = this.PlayerInput.actions["DiscardPowerup"];
        EgoBoost = this.PlayerInput.actions["EgoBoost"];
        PauseButton = this.PlayerInput.actions["Pause"];
        
        Inputs.DeviceCheck.MouseUsed.performed += _ => { DeviceCheck.GamepadInUse = false; crosshair.EnableCrosshair(); };
        Inputs.DeviceCheck.KeyboardUsed.performed += _ => { DeviceCheck.GamepadInUse = false; crosshair.EnableCrosshair(); };
        Inputs.DeviceCheck.GamepadUsed.performed += _ => { DeviceCheck.GamepadInUse = true; crosshair.DisableCrosshair(); Cursor.visible = false; };
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
        DeviceCheck.GamepadInUse = false;
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
        if (!DeviceCheck.GamepadInUse && this.PlayerInput.enabled) aimPos = Camera.main.ScreenToWorldPoint(mousePosition.ReadValue<Vector2>());
        else if (DeviceCheck.GamepadInUse && this.PlayerInput.enabled) aimPos = StickPosition.ReadValue<Vector2>();
        else aimPos = Camera.main.ScreenToWorldPoint(Inputs.UI.Point.ReadValue<Vector2>());

        return aimPos;
    }
}
