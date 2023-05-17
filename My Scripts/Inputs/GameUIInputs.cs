using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class GameUIInputs : MonoBehaviour
{
    ControlsManager controls;
    [SerializeField] private GameObject pauseMenu;

    void Start()
    {
        controls = FindObjectOfType<ControlsManager>();
        controls.Inputs.UI.Pause.performed += _ => PauseMenu();
        controls.PauseButton.performed += _ => PauseMenu();
    }
    
    void PauseMenu()
    {
        if (pauseMenu != null)
        {
            if (pauseMenu.activeInHierarchy) ClosePauseMenu();
            else OpenPauseMenu();
        }        
    }
    void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        controls.DisablePlayerControls();
        GameManager.Instance.PauseGame();
        Cursor.visible = true;
    }
    
    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        controls.EnablePlayerControls();
        GameManager.Instance.ResumeGame();
        Cursor.visible = false;
    }
}
