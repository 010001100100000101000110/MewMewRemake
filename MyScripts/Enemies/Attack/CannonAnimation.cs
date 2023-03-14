using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAnimation : MonoBehaviour
{
    BrontoShoot shoot;
    Animator anim;
    void Start()
    {
        shoot = GetComponentInParent<BrontoShoot>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        anim.SetBool("isShooting", shoot.isShooting);
    }

    public void Shoot()
    {
        shoot.Shoot();
    }
}
