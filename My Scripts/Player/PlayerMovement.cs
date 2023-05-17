using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    PlayerHelper helper;    
    public float activeMovementSpeed;
    
    public float regMoveSpeed { get; private set; }

    public Vector3 movementDirection => helper.Controls.Movement.ReadValue<Vector2>().normalized;

    #region Dash things

    Vector2 dashDirection;
    public int dashCharges { get; private set; } = 3;
    public int maxDashCharges { get; private set; }
    bool isDashing;
    float timer;
    float dashCounter;
    [SerializeField]float dashChargeTimer;


    public delegate void onDashChange();
    public onDashChange dashChange;
    #endregion

   
    private void Start()
    {
        

        helper.Controls.Dash.performed += _ => Dash();

        activeMovementSpeed = helper.Stats.MoveSpeed;
        regMoveSpeed = helper.Stats.MoveSpeed;
        DashInitialize();
        helper.SentryMode.OnSentryModeStatusChange += SentryModeMovementSpeed;
    }

    private void Awake()
    {
        helper = GetComponent<PlayerHelper>();

    }

    //private void OnEnable()
    //{
    //    helper.Shoot.PlayerShot += TempSlow;
    //}
    void Update()
    {
        DashTimer();
        DashCharge();
        Animate();
        SlowTimer();
        activeMovementSpeed = isDashing ? helper.Stats.DashSpeed : helper.Stats.MoveSpeed;
    }

    [SerializeField] float firingSlowAmount;
    float slowTimer;
    bool isSlowed;

    private void SlowTimer()
    {
        if (isSlowed)
        {
            slowTimer -= Time.deltaTime;
            if (slowTimer <= 0)
            {
                isSlowed = false;
            }
        }

    }

    private void TempSlow()
    {
        
        slowTimer = helper.Shoot.TimeBetweenShots;
        isSlowed = true;

    }

    void FixedUpdate()
    {
        Movement();
    }
    private void OnDisable()
    {
        helper.SentryMode.OnSentryModeStatusChange -= SentryModeMovementSpeed;
        //helper.Shoot.PlayerShot -= TempSlow;
    }

    void Movement()
    {
        Vector3 direction;
        if (isDashing) direction = dashDirection;
        else direction = movementDirection;
        helper.Rb.MovePosition(transform.position + (direction * activeMovementSpeed * Time.deltaTime));        
    }

    void Animate()
    {
        helper.Animator.SetFloat("Speed", movementDirection.magnitude);
        helper.Animator.SetBool("IsDashing", isDashing);
    }
    private void DashInitialize()
    {
        maxDashCharges = helper.Stats.DashCharges;
        dashCharges = maxDashCharges;
        dashChange?.Invoke();
    }
    void Dash()
    {
        if (dashCharges > 0 && !helper.SentryMode.IsActive)
        {
            activeMovementSpeed = helper.Stats.DashSpeed;
            if (movementDirection.x == 0 && movementDirection.y == 0) dashDirection = (new Vector3 (helper.Controls.GetAimPosition().x, helper.Controls.GetAimPosition().y, 0) - transform.position).normalized;
            else dashDirection = movementDirection;
            dashCounter = helper.Stats.DashLength;
            isDashing = true;
            dashCharges--;
            dashChange?.Invoke();
            helper.Health.EnableDashInv();
            SFXManager.RequestSound(dashSFX);
            EffectManager.instance.CameraShake(0.02f);
        }            
    }

    [SerializeField] AudioEvent dashSFX;
    void DashCharge()
    {
        if (dashCharges < maxDashCharges)
        {
            timer += Time.deltaTime;
            if (timer >= dashChargeTimer) AddCharge();
        }
    }

    void AddCharge()
    {
        if (dashCharges < maxDashCharges) dashCharges++;
        timer = 0;
        dashChange?.Invoke();
    }
    void DashTimer()
    {
        if (isDashing)
        {
            dashCounter -= Time.deltaTime;
            if (dashCounter <= 0)
            {
                activeMovementSpeed = helper.Stats.MoveSpeed;
                isDashing = false;
                helper.Health.DisableDashInv();
            }
        }
    }

    private void SentryModeMovementSpeed()
    {
        if (helper.SentryMode.IsActive && !isDashing)
        {
            activeMovementSpeed = helper.Stats.sentryMode.MoveSpeed;
        }
        else if (!helper.SentryMode.IsActive && !isDashing)
        {
            activeMovementSpeed = helper.Stats.MoveSpeed;
        }
    }

    private void UpdateDashCharges()
    {
        maxDashCharges = helper.Stats.DashCharges;
        dashCharges = maxDashCharges;
        dashChange?.Invoke();
    }
}