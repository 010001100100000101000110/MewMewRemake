using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRexShoot : MonoBehaviour
{
    EnemyHelper helper;
    
    [SerializeField] Transform barrel;

    float nextFire;
    float fireRate;
    float maxRange;

    float projectileSpeed = 5;
    float timeBetweenShots;
    int shotsFired;
    float reloadTime;
    public bool IsReloading { get; private set; }
    bool isShooting;

    private void OnEnable()
    {
        StopAllCoroutines();
        shotsFired = 0;
        IsReloading = false;
    }
    void Start()
    {
        helper = GetComponent<EnemyHelper>();        
        maxRange = helper.Stats.Range;
        fireRate = helper.Stats.ShotsPerMinute;
        projectileSpeed = helper.Stats.ShotSpeed;
        reloadTime = helper.Stats.AttackCooldown;
        DefineFireRate();
    }
    void DefineFireRate()
    {
        timeBetweenShots = 1 / (fireRate / 60);
    }

    void Update()
    {
        if (!helper.TGManager.TopGunning)
        {
            RotateBarrel();
            if (!IsReloading)
            {
                if (helper.DistanceToPlayer() < maxRange) isShooting = true;
                if (isShooting)
                {
                    if (shotsFired < 2) Shoot();
                    if (shotsFired >= 2) StartCoroutine(ReloadTimer());
                }
            }
        }
        else
        {
            isShooting = false;
            IsReloading = false;
            shotsFired = 0;
        }
            
    }
    
    IEnumerator ReloadTimer()
    {
        isShooting = false;
        IsReloading = true;
        yield return new WaitForSecondsRealtime(reloadTime);
        shotsFired = 0;
        IsReloading = false;        
    }
    
    void Shoot()
    {
        if (Time.time < nextFire) return;
        nextFire = Time.time + timeBetweenShots;
        shotsFired++;

        GameObject go = helper.Manager.ProjectileManager.GetProjectile();
        go.GetComponent<OnPlayerHit>().SetMaxDistance(helper.Stats.ProjectileRange);
        float facingRotation = BarrelRotation();
        go.transform.localRotation = Quaternion.Euler(0, 0, facingRotation);

        go.transform.position = barrel.position;
        OnPlayerHit onPlayerHit = go.GetComponent<OnPlayerHit>();
        onPlayerHit.SetEnemyDamage(helper.Stats.Damage);
        //onPlayerHit.SetMaxDistance(helper.Stats.Range);
        go.SetActive(true);
        go.GetComponent<Rigidbody2D>().AddForce(barrel.right * projectileSpeed, ForceMode2D.Impulse);
    }
    public Vector3 Direction()
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
}
