using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;
using TMPro;

public class SwapSticksToggle : MonoBehaviour
{
    Toggle swapToggle;
    PlayerInput inputs;
    public InputAction StickPosition { get; private set; }
    public InputAction Movement { get; private set; }
    public InputAction Shoot { get; private set; }

    private void OnEnable()
    {
        swapToggle.isOn = IntToBool(PlayerPrefs.GetInt("GamepadToggle"));
    }
    void Awake()
    {
        inputs = FindObjectOfType<PlayerInput>();
        swapToggle = GetComponent<Toggle>();
        StickPosition = inputs.actions["StickPosition"];
        Movement = inputs.actions["Movement"];
        Shoot = inputs.actions["Shoot"];
        swapToggle.isOn = IntToBool(PlayerPrefs.GetInt("GamepadToggle"));
        SetSticks();
    }

    public void SetSticks()
    {
        PlayerPrefs.SetInt("GamepadToggle", BoolToInt(swapToggle.isOn));
        if (swapToggle.isOn) InvertedSticks();
        else DefaultSticks();
    }
    void DefaultSticks()
    {
        Movement.ChangeBinding(5).WithPath("<Gamepad>/leftStick");
        StickPosition.ChangeBinding(0).WithPath("<Gamepad>/rightStick");
        Shoot.ChangeBinding(1).WithPath("<Gamepad>/rightStick");
    }
    void InvertedSticks()
    {
        Movement.ChangeBinding(5).WithPath("<Gamepad>/rightStick");
        StickPosition.ChangeBinding(0).WithPath("<Gamepad>/leftStick");
        Shoot.ChangeBinding(1).WithPath("<Gamepad>/leftStick");
    }
    bool IntToBool(int value)
    {
        if (value != 0)
            return true;
        else
            return false;
    }

    int BoolToInt(bool value)
    {
        if (value)
            return 1;
        else
            return 0;
    }
}
