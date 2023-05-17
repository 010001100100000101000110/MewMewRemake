using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnkyloShoot : MonoBehaviour
{
    EnemyHelper helper;

    [SerializeField] Transform barrel;

    float nextFire;
    float maxRange;
    float fireRate;
    float timeBetweenShots;

    float projectileSpeed = 5;
    public bool IsShooting { get; private set; }


    void Start()
    {
        helper = GetComponent<EnemyHelper>();
        fireRate = helper.Stats.ShotsPerMinute;
        maxRange = helper.Stats.Range;
        projectileSpeed = helper.Stats.ShotSpeed;
        DefineFireRate();
    }
    void Update()
    {
        if (!helper.TGManager.TopGunning)
        {
            RotateBarrel();
            if (helper.DistanceToPlayer() < maxRange)
            {
                if (Time.time < nextFire) return;
                IsShooting = true;
                if (helper.Animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot")) Shoot();
            }
            else IsShooting = false;
        }
        else
        {
            IsShooting = false;
        }        
    }
    void DefineFireRate()
    {
        timeBetweenShots = 1 / (fireRate / 60);
    }
    public void Shoot()
    {
        nextFire = Time.time + timeBetweenShots;
        GameObject go = helper.Manager.ProjectileManager.GetProjectile();
        go.GetComponent<OnPlayerHit>().SetMaxDistance(helper.Stats.ProjectileRange);

        float facingRotation = BarrelRotation();
        go.transform.localRotation = Quaternion.Euler(0, 0, facingRotation);

        go.transform.position = barrel.position;
        go.GetComponent<OnPlayerHit>().SetEnemyDamage(helper.Stats.Damage);
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
