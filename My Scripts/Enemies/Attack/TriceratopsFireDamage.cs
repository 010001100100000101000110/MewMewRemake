using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriceratopsFireDamage : MonoBehaviour
{
    float damage;
    float nextDamage;
    [SerializeField] float timeBetweenDamage;

    private void Start()
    {
        damage = GetComponentInParent<EnemyHelper>().Stats.Damage;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerHealth playerHealth;
        if (collision.gameObject.TryGetComponent<PlayerHealth>(out playerHealth))
        {
            if (Time.time < nextDamage) return;
            nextDamage = Time.time + timeBetweenDamage;
            playerHealth.TakeDamage(damage, gameObject);
        }
    }
}
