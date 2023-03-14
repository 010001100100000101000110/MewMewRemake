using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrontoPoisonCloudBehaviour : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] float damage;
    float nextDamage;
    [SerializeField] float timeBetweenDamage;

    void Start()
    {
        StartCoroutine(Duration(duration));
    }
    IEnumerator Duration(float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
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
