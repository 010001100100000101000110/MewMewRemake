using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    ControlsManager inputs;
    void Start()
    {
        inputs = FindObjectOfType<ControlsManager>();
        MouseInvisible();
    }

    void Update()
    {
        this.transform.position =   inputs.GetAimPosition();
    }

    void MouseInvisible()
    {
        Cursor.visible = false;
    }

    void MouseVisible()
    {
        Cursor.visible = true;
    }

    public void EnableCrosshair()
    {
        gameObject.SetActive(true);
    }

    public void DisableCrosshair()
    {
        gameObject.SetActive(false);
    }

}
