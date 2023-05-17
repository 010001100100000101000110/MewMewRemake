using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriceratopsFireTrailBehaviour : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] float damage;
    float nextDamage;
    [SerializeField] float timeBetweenDamage;
    FireTrailPool fireTrails;
    

    private void OnEnable()
    {
        StartCoroutine(Duration(duration));
    }

    void Start()
    {
        fireTrails = FindObjectOfType<FireTrailPool>();        
    }    

    IEnumerator Duration(float duration)
    {
        yield return new WaitForSeconds(duration);
        fireTrails.ReturnFireTrail(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerHealth playerHealth;
        if (collision.gameObject.TryGetComponent<PlayerHealth>(out playerHealth))
        {
            if (!playerHealth.FDCoroutineOn) playerHealth.StartCoroutine(playerHealth.StartFireDamage());
        }
    }
}
