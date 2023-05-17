using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelInputs : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject selectedButton;
    DeviceChecking deviceCheck;

    private void Awake()
    {
        deviceCheck = FindObjectOfType<DeviceChecking>();
    }
    private void OnEnable()
    {
        deviceCheck.UseGamepad += ActivateCurrentButton;
        deviceCheck.UseMouse += DeactivateCurrentButton;
        if (deviceCheck.GamepadInUse) EventSystem.current.SetSelectedGameObject(selectedButton);
    }
    private void OnDisable()
    {
        deviceCheck.UseGamepad -= ActivateCurrentButton;
        deviceCheck.UseMouse -= DeactivateCurrentButton;
    }
    void ActivateCurrentButton()
    {
        if (panel.activeInHierarchy) EventSystem.current.SetSelectedGameObject(selectedButton);
    }
    void DeactivateCurrentButton()
    {
        if (panel.activeInHierarchy) EventSystem.current.SetSelectedGameObject(null);
    }
}
