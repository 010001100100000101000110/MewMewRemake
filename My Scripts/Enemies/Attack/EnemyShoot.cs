using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    //GameObject player;
    //[field: SerializeField] public EnemyStatsSO stats { get; private set; }
    //EnemyController controller;
    EnemyHelper helper;

    [SerializeField] GameObject enemyProjectile;
    float nextFire;
    float fireRate;
    float maxRange;
    
    [SerializeField] Transform barrel;   

    float projectileSpeed = 5;
    int projectileAmount;
    float timeBetweenShots;
    float projectileSpread;
    [SerializeField] float minRangeDistance;
    public bool inRange;

    

    [SerializeField] int shotsFired;

    public bool IsReloading;

    [SerializeField] float reloadCooldown;
    [SerializeField] float shotCooldown;
    float tempCooldown;

    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        //controller = GetComponent<EnemyController>();

        helper = GetComponent<EnemyHelper>();       
        fireRate = helper.Stats.ShotsPerMinute;
        maxRange = helper.Stats.Range;
        projectileSpeed = helper.Stats.ShotSpeed;
        DefineFireRate();

        tempCooldown = shotCooldown;
    }

    void DefineFireRate()
    {
        timeBetweenShots = 1 / (fireRate / 60);
    }


    void Update()
    {
        RotateBarrel();
        if (DistanceToPlayer() < maxRange)
        {
            if (helper.Stats.Type == EnemyType.T_Rex)
            {
                TRexShooting();
            }
            if (Time.time < nextFire) return;
            if (helper.Stats.Type == EnemyType.Pterodactyl)
            {
                PterodactylShooting();
            }
           
        }
    }

    float DistanceToPlayer()
    {
        float distance = Vector2.Distance(transform.position, helper.Player.position);
        return distance;
    }

    //void Shoot()
    //{
    //    nextFire = Time.time + timeBetweenShots;
    //    if (helper.Stats != null)
    //    {
    //        switch (helper.Stats.ProjectileAmount)
    //        {
    //            case 1:
    //                ShootSingle();
    //                break;

    //            default:
    //                ShootShotgun();
    //                break;
    //        }
    //    }
    //}
    IEnumerator ReloadTimer()
    {
        IsReloading = true;
        yield return new WaitForSecondsRealtime(reloadCooldown);
        shotsFired = 0;

        IsReloading = false;
        
    }

   
    void ShootSingle()
    {
        Vector3 spawnPos = new Vector3(barrel.position.x, barrel.position.y, barrel.position.z);
        GameObject go = Instantiate(enemyProjectile);
        go.transform.position = barrel.position;
        go.SetActive(true);
        go.GetComponent<Rigidbody2D>().AddForce(barrel.right * projectileSpeed, ForceMode2D.Impulse);
    }
    void ShootShotgun()
    {
        //projectileAmount = helper.Stats.ProjectileAmount;
        //projectileSpread = helper.Stats.ProjectileAmount * 8;
        //Quaternion gunRotation = barrel.rotation;
        float facingRotation = BarrelRotation();
        float startRotation = facingRotation + projectileSpread / 2f;
        float angleIncrease = projectileSpread / (projectileAmount - 1);
        Vector3 spawnPos = new Vector3(barrel.position.x, barrel.position.y, barrel.position.z);

        for (int i = 0; i < projectileAmount; i++)
        {
            float tempRot = startRotation - angleIncrease * i;

            GameObject bullet = Instantiate(enemyProjectile);
            bullet.transform.position = barrel.position;
            bullet.transform.localRotation = Quaternion.Euler(0, 0, tempRot);
            Vector3 direction = bullet.transform.right;
            bullet.SetActive(true);
            bullet.GetComponent<Rigidbody2D>().AddForce((bullet.transform.right) * helper.Stats.ShotSpeed, ForceMode2D.Impulse);
        }

    }
    void PterodactylShooting()
    {
        if ((helper.Agent.velocity.x < 0 && transform.position.x < helper.Player.position.x) || (helper.Agent.velocity.x > 0 && transform.position.x > helper.Player.position.x) || DistanceToPlayer() < minRangeDistance) inRange = false;
        else inRange = true;
        //if (inRange) Shoot();
    }

    void TRexShooting()
    {
        if (!IsReloading && !(tempCooldown <= 0) && shotsFired < 2)
        {
           // Shoot();
            tempCooldown -= Time.deltaTime;
            shotsFired++;
        }
        if ( shotsFired >= 2 && !IsReloading) StartCoroutine(ReloadTimer());


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
