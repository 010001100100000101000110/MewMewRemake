using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSprite : MonoBehaviour
{
    [SerializeField] bool flipY;
    Camera mainCam;
    PlayerHelper helper;

    void Start()
    {
        helper = GetComponentInParent<PlayerHelper>();
        mainCam = Camera.main;
    }

    void Update()
    {
        Flip();
    }
    private void Flip()
    {
        if (!helper.Controls.DeviceCheck.GamepadInUse)
        {
            if (mainCam.WorldToScreenPoint(helper.Controls.GetAimPosition()).x >= mainCam.WorldToScreenPoint(transform.position).x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (flipY) transform.localScale = new Vector3(-1, -1, 1);
            else transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            if (helper.Controls.GetAimPosition().x > 0) transform.localScale = new Vector3(1, 1, 1);
            if (helper.Controls.GetAimPosition().x < 0) 
            {
                if (flipY) transform.localScale = new Vector3(-1, -1, 1);
                else transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
}
