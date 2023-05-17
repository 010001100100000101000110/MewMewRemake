using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyAttack : MonoBehaviour
{
    EnemyHelper helper;
    [SerializeField] Collider2D edgeCollider;
    [SerializeField] Collider2D circleCollider;
    bool isExploding;
    void Start()
    {
        helper = GetComponent<EnemyHelper>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth playerHealth;
        if (collision.gameObject.TryGetComponent<PlayerHealth>(out playerHealth))
        {
            if (isExploding) playerHealth.TakeDamage(helper.Stats.Damage, gameObject);
            StartCoroutine(StartExplode());            
        }
    }
    public IEnumerator StartExplode()
    {
        helper.Animator.SetTrigger("Explode");
        edgeCollider.enabled = false;
        helper.Agent.isStopped = true;

        yield return new WaitForSecondsRealtime(0.9f);
        Destroy(gameObject);
    }

    void Explode()
    {        
        circleCollider.enabled = true;
        isExploding = true;
    }    
}
