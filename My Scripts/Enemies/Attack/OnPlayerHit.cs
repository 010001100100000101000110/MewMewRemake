using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ProjectileType { basic, boss }

public class OnPlayerHit : MonoBehaviour
{
    GameObject enemyWhoShotThis;
    float damage;
    EffectManager effects;
    EnemyProjectileManager projectileManager;
    float maxDistance = 14;
    [SerializeField]float distanceTraveled;
    Vector2 initialPosition;
    
    [SerializeField] ProjectileType type = ProjectileType.basic;

    public void SetManager (EnemyProjectileManager manager)
    {
        projectileManager = manager;
    }

    private void OnEnable()
    {
        distanceTraveled = 0;
        initialPosition = transform.position;
        if (projectileManager == null) return;
        projectileManager.WaveManager.WaveEnd += ReturnToPool;
    }

    private void OnDisable()
    {
        if (projectileManager == null) return;
        projectileManager.WaveManager.WaveEnd -= ReturnToPool;
    }

    void ReturnToPool()
    {
        if (type == ProjectileType.basic)
            projectileManager.ReturnProjectileToPool(gameObject);

        else projectileManager.ReturnBossProjectileToPool(gameObject);
    }

    void Start()
    {
        effects = FindObjectOfType<EffectManager>();
    }

    public void SetEnemyDamage(float damage)
    {
        this.damage = damage;

    }

    public void SetMaxDistance(float distance)
    {
        if (distance > 0) maxDistance = distance;
    }

    private void Update()
    {
        distanceTraveled = Vector3.Distance(initialPosition, transform.position);
        if (Vector3.Distance(initialPosition, transform.position) >= maxDistance)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth playerHealth;
        if (collision.CompareTag("Shield"))
        {
            FloatingTextManager.InstantiateFloatingText(transform.position, "Blocked", FloatingTextManager.Instance.Blocked);
            gameObject.SetActive(false);
            ReturnToPool();
            return;
        }
        else if (collision.gameObject.TryGetComponent<PlayerHealth>(out playerHealth))
        {
            playerHealth.TakeDamage(damage, enemyWhoShotThis);
            
            gameObject.SetActive(false);
            ReturnToPool();
        }
    }
}
