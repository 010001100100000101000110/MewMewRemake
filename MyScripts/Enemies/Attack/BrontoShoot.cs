using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrontoShoot : MonoBehaviour
{
    EnemyHelper helper;
    [SerializeField] Transform barrel;
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject poisonCloud;

    float projectileSpeed;
    float maxRange;

    float nextFire;
    float fireRate;
    float timeBetweenShots;

    float nextCloud;
    [SerializeField] float cloudFireRate;
    float timeBetweenClouds;

    public bool isShooting { get; private set; }

    PlayerMovement playerMovement;

    void Start()
    {
        helper = GetComponent<EnemyHelper>();
        playerMovement = helper.Player.GetComponent<PlayerMovement>();
        projectileSpeed = helper.Stats.ShotSpeed;
        maxRange = helper.Stats.Range;
        fireRate = helper.Stats.ShotsPerMinute;
        DefineFireRate();
        DefineCloudRate();
    }

    void Update()
    {
        if (helper.DistanceToPlayer() < maxRange && helper.DistanceToPlayer() > 4)
        {
            if (Time.time < nextFire)
            {
                isShooting = false;
                return;
            }
            isShooting = true;
        }
        else if (helper.DistanceToPlayer() <= 4)
        {
            if (Time.time < nextCloud)
            {
                isShooting = false;
                return;
            }
            CloudAttack();
        }
        else isShooting = false;

    }
    void DefineFireRate()
    {
        timeBetweenShots = 1 / (fireRate / 60);
    }
    void DefineCloudRate()
    {
        timeBetweenClouds = 1 / (cloudFireRate / 60);
    }
    public void Shoot()
    {
        if (helper.Stats != null)
        {
            nextFire = Time.time + timeBetweenShots;
            GameObject go = Instantiate(projectile);
            go.transform.position = barrel.position;
            go.GetComponent<BrontoProjectileBehaviour>().SetProperties(helper.Player.position, projectileSpeed, helper.Stats.Damage, gameObject);
            go.GetComponent<BrontoProjectileBehaviour>().SetPlayerProperties(helper.Player.GetComponent<PlayerMovement>().regMoveSpeed, playerMovement.movementDirection);
            go.SetActive(true);
        }
    }

    void CloudAttack()
    {
        nextCloud = Time.time + timeBetweenClouds;
        GameObject go = Instantiate(poisonCloud);
        go.transform.position = this.transform.position;
        go.SetActive(true);
    }
}
