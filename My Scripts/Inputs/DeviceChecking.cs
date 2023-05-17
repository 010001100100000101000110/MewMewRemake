using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceChecking : MonoBehaviour
{
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
}
