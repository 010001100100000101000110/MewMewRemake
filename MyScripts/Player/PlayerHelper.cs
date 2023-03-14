using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHelper : MonoBehaviour
{
    public Animator Animator { get; private set; }
    public PlayerHealth Health { get; private set; }
    public PlayerShoot Shoot { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public CapsuleCollider2D Collider { get; private set; }
    public RotateBarrel RotBarrel { get; private set; }
    public PlayerStats Stats { get; private set; }
    public ControlsManager Controls { get; private set; }
    public SentryMode SentryMode { get; private set; }

    public ProjectileManager ProjectileManager { get; private set; }

    private void Awake()
    {
        this.Animator = GetComponent<Animator>();
        Health = GetComponent<PlayerHealth>();
        Shoot = GetComponent<PlayerShoot>();
        Rb = GetComponent<Rigidbody2D>();
        Collider = GetComponent<CapsuleCollider2D>();
        RotBarrel = GetComponentInChildren<RotateBarrel>();
        Stats = GetComponent<PlayerStats>();
        Controls = FindObjectOfType<ControlsManager>();
        SentryMode = FindObjectOfType<SentryMode>();
        ProjectileManager = FindObjectOfType<ProjectileManager>();
        this.gameObject.tag = "Player";
    }
}
