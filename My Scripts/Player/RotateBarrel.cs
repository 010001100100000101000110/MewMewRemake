using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBarrel : MonoBehaviour
{
    ControlsManager controls;
    Vector3 aimPos;
    bool stickAiming;

    [SerializeField] Vector2 lastAimPos;

    private void Start()
    {
        controls = FindObjectOfType<ControlsManager>();
    }

    void Update()
    {
        if (controls.StickPosition.ReadValue<Vector2>().magnitude > 0) lastAimPos = controls.StickPosition.ReadValue<Vector2>();
        if (controls.StickPosition.ReadValue<Vector2>() != Vector2.zero) stickAiming = true;
        else stickAiming = false;

        transform.rotation = Quaternion.Euler(0, 0, Rotation());
    }

    public float Rotation()
    {
        aimPos = controls.GetAimPosition();
        Vector3 direction;

        if (!controls.DeviceCheck.GamepadInUse) direction = aimPos - transform.position;
        else if (!stickAiming) direction = lastAimPos;
        else direction = aimPos;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return angle;
    }
}
