using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelInputs : MonoBehaviour
{
    [SerializeField] ControlsManager controls;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject selectedButton;

    private void Awake()
    {
        controls = FindObjectOfType<ControlsManager>();
    }
    private void OnEnable()
    {        
        controls.UseGamepad += ActivateCurrentButton;
        controls.UseMouse += DeactivateCurrentButton;
        if (controls.GamepadInUse) EventSystem.current.SetSelectedGameObject(selectedButton);
    }
    private void OnDisable()
    {
        controls.UseGamepad -= ActivateCurrentButton;
        controls.UseMouse -= DeactivateCurrentButton;
        EventSystem.current.SetSelectedGameObject(null);
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
