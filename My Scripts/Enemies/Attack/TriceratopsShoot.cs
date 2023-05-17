using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriceratopsShoot : MonoBehaviour
{
    EnemyHelper helper;
    TriceratopsController controller;

    [SerializeField] GameObject fireBall;   
    [SerializeField] Transform barrel;

    [SerializeField] int projectileAmount;
    [SerializeField] int shootBurstAmount;

    [SerializeField] float shootingCooldown;
    [SerializeField] int maxTimesShot;
    [SerializeField] float spread = 8;

    float nextFire;
    float fireRate;
    float timeBetweenShots;
    float projectileSpeed = 5;
    int shotsFired;
    int timesShot;
    public bool IsFiring { get; private set; }
    bool canShoot; 
    bool startShooting;

    void Start()
    {
        controller = GetComponent<TriceratopsController>();
        helper = GetComponent<EnemyHelper>();
        projectileSpeed = helper.Stats.ShotSpeed;
        fireRate = helper.Stats.ShotsPerMinute;
        DefineFireRate();
        canShoot = true;
    }
    public void ShootUpdate()
    {
        helper.Animator.SetBool("IsFiring", IsFiring);

        if (!helper.TGManager.TopGunning)
        {
            if (!controller.Sprinting)
            {
                RotateBarrel();

                if (shotsFired >= shootBurstAmount) StartCoroutine(ShootingCooldown(shootingCooldown));
                if (timesShot > maxTimesShot) controller.ChangeState(controller.IdleState);

                if (controller.InRange() && !controller.NotInRange() && canShoot && shotsFired < shootBurstAmount)
                {
                    if (!IsFiring) timesShot++;
                    IsFiring = true;                    
                }

                if (startShooting) ShootShotgun();

                if (controller.Sprinting)
                {
                    ResetShooting();
                }
            }
            else ResetShooting();
        }
        else controller.ChangeState(controller.IdleState);
    }
    public void ShootShotgun()
    {
        if (Time.time < nextFire) return;
        shotsFired++;
        nextFire = Time.time + timeBetweenShots;
        float projectileSpread = projectileAmount * spread;
        float facingRotation = BarrelRotation();
        float startRotation = facingRotation + projectileSpread * 0.5f;
        float angleIncrease = projectileSpread / (projectileAmount - 1);

        for (int i = 0; i < projectileAmount; i++)
        {
            GameObject bullet = helper.Manager.ProjectileManager.GetBossProjectile();
            OnPlayerHit hit = bullet.GetComponent<OnPlayerHit>();
            hit.SetEnemyDamage(helper.Stats.Damage);
            hit.SetMaxDistance(helper.Stats.ProjectileRange);


            float tempRot = startRotation - angleIncrease * i;
            bullet.transform.position = barrel.transform.position;
            bullet.transform.rotation = Quaternion.Euler(0, 0, tempRot);
            bullet.SetActive(true);
            bullet.GetComponent<Rigidbody2D>().AddForce((bullet.transform.right) * projectileSpeed, ForceMode2D.Impulse);
        }
    }

    IEnumerator ShootingCooldown(float duration)
    {
        //timesShot++;
        IsFiring = false;
        startShooting = false;
        canShoot = false;
        shotsFired = 0;
        yield return new WaitForSecondsRealtime(duration);        
        canShoot = true;
    }

    void ResetShooting()
    {
        shotsFired = 0;
        IsFiring = false;
        startShooting = false;
    }

    public void ResetAll()
    {
        IsFiring = false;
        canShoot = true;
        startShooting = false;
        helper.Animator.SetBool("IsFiring", false);
        shotsFired = 0;        
        timesShot = 0;
    }
    void DefineFireRate()
    {
        timeBetweenShots = 1 / (fireRate / 60);
    }

    Vector3 Direction()
    {
        return helper.Player.position - barrel.position;
    }

    void RotateBarrel()
    {
        Vector3 dir = Direction();
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        barrel.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    float BarrelRotation()
    {
        Vector3 direction = helper.Player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return angle;
    }

    //animation event
    void StartShooting()
    {        
        startShooting = true;
    }   
}
