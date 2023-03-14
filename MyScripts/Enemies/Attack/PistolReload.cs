using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolReload : MonoBehaviour
{
    Animator animator;
    TRexShoot shoot;
    void Start()
    {
        animator = GetComponent<Animator>();
        shoot = GetComponentInParent<TRexShoot>();
    }
    void Update()
    {
        animator.SetBool("Reloading", shoot.IsReloading);
    }
}
