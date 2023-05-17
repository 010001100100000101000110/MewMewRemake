using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallBehaviour : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(DestroyFireBall());
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    IEnumerator DestroyFireBall()
    {
        yield return new WaitForSecondsRealtime(3);
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth playerHealth;
        if (collision.CompareTag("Shield"))
        {
            FloatingTextManager.InstantiateFloatingText(transform.position, "Damage Blocked", FloatingTextManager.Instance.Blocked);
            gameObject.SetActive(false);
            return;
        }
        else if (collision.gameObject.TryGetComponent<PlayerHealth>(out playerHealth))
        {
            playerHealth.TakeDamage(3, gameObject);

            gameObject.SetActive(false);
        }
    }
}
