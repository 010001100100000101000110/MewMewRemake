using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PteroShoot : MonoBehaviour
{
    EnemyHelper helper;

    [SerializeField] Transform barrel;

    float nextFire;
    float fireRate;
    float maxRange;

    float projectileSpeed = 5;
    float timeBetweenShots;
    [SerializeField] float minRangeDistance;

    void Start()
    {
        helper = GetComponent<EnemyHelper>();
        fireRate = helper.Stats.ShotsPerMinute;
        maxRange = helper.Stats.Range;
        projectileSpeed = helper.Stats.ShotSpeed;
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
            if (helper.DistanceToPlayer() < maxRange)
            {
                if (Time.time < nextFire) return;
                if (InRange()) Shoot();
            }
        }
            
    }
    bool InRange()
    {
        if ((helper.Agent.velocity.x < 0 && transform.position.x < helper.Player.position.x) || (helper.Agent.velocity.x > 0 && transform.position.x > helper.Player.position.x) || IsTooClose()) 
        return false;
        else return true;
    }

    bool IsTooClose()
    {
        if ((helper.Agent.velocity.x < 0 && (transform.position.x - helper.Player.transform.position.x) < minRangeDistance) || (helper.Agent.velocity.x > 0 && (helper.Player.transform.position.x - transform.position.x) < minRangeDistance)) return true;
        else return false;
    }

    void Shoot()
    {
        if (helper.Stats != null)
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
    }

    private float BarrelRotation()
    {
        Vector3 direction = helper.Player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return angle;
    }

    public Vector3 direction()
    {
        return helper.Player.position - barrel.position;
    }

    void RotateBarrel()
    {
        Vector3 dir = direction();
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        barrel.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
