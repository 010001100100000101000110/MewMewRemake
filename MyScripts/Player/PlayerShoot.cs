using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;

public class PlayerShoot : MonoBehaviour
{
    PlayerHelper helper;    
    [SerializeField] GameObject barrel;
    [SerializeField] ProjectileManager projectileManager;
    [SerializeField] AudioEvent shotSFX;
    [SerializeField] SentryMode_Behaviour sentryMode;
   
    private float nextFire;
    private float timeBetweenShots;

    public float TimeBetweenShots => timeBetweenShots;

    private int projectileSpread;
    private int projectileAmount;

    bool pressed;

    [SerializeField] Animator muzzleFire;
    [SerializeField] AnimationClip[] animationClips;

    public delegate void Shot();
    public Shot PlayerShot;
    void Start()
    {
        helper = GetComponent<PlayerHelper>();

        helper.Controls.Shoot.performed += _ => pressed = true;
        helper.Controls.Shoot.canceled += _ => pressed = false;

        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        MouseInput();
    }

    void MouseInput()
    {
        if (pressed)
        {
            if (Time.time < nextFire) return;
            Shoot();
        }
    }
    void Shoot()
    {
        timeBetweenShots = 1 / (helper.Stats.ShotsPerMinute / 60);
        nextFire = Time.time + timeBetweenShots;
        sentryMode.IsFiring(pressed);

        if (helper.Stats != null)
        {
            switch (helper.Stats.ProjectileAmount)
            {
                case 1:
                    ShootSingle();
                    break;

                default:
                    ShootShotgun();
                    break;
            }
        }

        shotSFX.Play(source);
        //SFXManager.PlaySound(GunSound);
        MuzzleEffect();
        PlayerShot?.Invoke();
        EffectManager.instance.CameraKnockback(-barrel.transform.right);
    }
    AudioSource source;
    void MuzzleEffect()
    {
        int x = Random.Range(0, animationClips.Length);
        string clip = animationClips[x].name;
        muzzleFire.Play(clip);
    }


    void ShootSingle()
    {
        GameObject bullet = projectileManager.GetProjectileFromPool();

        float facingRotation = helper.RotBarrel.Rotation();
        bullet.transform.localRotation = Quaternion.Euler(0, 0, facingRotation );

        bullet.transform.position = barrel.transform.position;

        bullet.GetComponent<PlayerProjectile>().SetMaxDistance(helper.Stats.ProjectileRange);
        bullet.SetActive(true);
        bullet.GetComponent<Rigidbody2D>().AddForce((bullet.transform.right) * helper.Stats.ProjectileSpeed, ForceMode2D.Impulse);
    }

    void ShootShotgun()
    {
        projectileAmount = helper.Stats.ProjectileAmount;
        projectileSpread = helper.Stats.ProjectileAmount * 8;
        float facingRotation = helper.RotBarrel.Rotation();
        float startRotation = facingRotation + projectileSpread / 2;
        float angleIncrease = projectileSpread / (projectileAmount - 1);

        for (int i = 0; i < projectileAmount; i++)
        {
            GameObject bullet = projectileManager.GetProjectileFromPool();
            float tempRot = startRotation - angleIncrease * i;
            bullet.transform.position = barrel.transform.position;
            bullet.transform.rotation = Quaternion.Euler(0, 0, tempRot);
            bullet.GetComponent<PlayerProjectile>().SetMaxDistance(helper.Stats.ProjectileRange);
            bullet.SetActive(true);
            bullet.GetComponent<Rigidbody2D>().AddForce((bullet.transform.right) * helper.Stats.ProjectileSpeed, ForceMode2D.Impulse);
        }
    }
}
