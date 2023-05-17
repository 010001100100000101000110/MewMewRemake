using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrontoProjectileBehaviour : MonoBehaviour
{
    Vector3 startPos;
    Vector3 targetPos;
    Vector2 playerPos;
    float speed = 10;
    [SerializeField] float arcHeight = 1;
    float damage;
    [SerializeField] GameObject poisonCloud;
    GameObject enemyWhoShotThis;
    Collider2D coll;
    Animator anim;
    EffectManager effects;

    [SerializeField] AnimationCurve travelCurve;

    float playerSpeed;
    Vector2 playerMovementDirection;

    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();        
        effects = FindObjectOfType<EffectManager>();
        coll.enabled = false;
        startPos = transform.position;
        targetPos = PredictedLandingSpot();
    }
    
    Vector2 PredictedLandingSpot()
    {
        Vector2 landingSpot = playerPos + playerMovementDirection.normalized * playerSpeed * 1.1f;
        return landingSpot;
    }
    void Update()
    {
        Trajectory();
    }

    void Trajectory()
    {
        float dist = targetPos.x - startPos.x;
        float testSpeed = Mathf.Abs(dist) / speed;

        float nextX = Mathf.MoveTowards(transform.position.x, targetPos.x, testSpeed * Time.deltaTime); 
        float baseY = Mathf.Lerp(startPos.y, targetPos.y, (nextX - startPos.x) / dist);
        float arc = arcHeight * (nextX - startPos.x) * (nextX - targetPos.x) / (-0.25f * dist * dist);
        Vector3 nextPos = new Vector3(nextX, baseY + arc, transform.position.z);

        transform.rotation = LookAt2D(nextPos - transform.position);
        transform.position = nextPos;

        if (nextPos == targetPos) Explode();
    }

    void Explode()
    {        
        anim.SetTrigger("Explode");
        coll.enabled = true;
        StartCoroutine(DestroyAfterTime(anim.GetCurrentAnimatorClipInfo(0).Length - 0.1f));
    }
    IEnumerator DestroyAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        GameObject cloud = Instantiate(poisonCloud);
        cloud.SetActive(true);
        cloud.transform.position = this.transform.position;
        gameObject.SetActive(false);
    }

    static Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }

    public void SetProperties(Vector3 destination, float speed, float damage, GameObject enemy)
    {
        this.playerPos = destination;
        this.speed = speed;
        this.damage = damage;
        enemyWhoShotThis = enemy;
    }

    public void SetPlayerProperties(float playerSpeed, Vector2 movementDirection)
    {
        this.playerSpeed = playerSpeed;
        this.playerMovementDirection = movementDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth playerHealth;
        if (collision.gameObject.TryGetComponent<PlayerHealth>(out playerHealth))
        {
            playerHealth.TakeDamage(damage, enemyWhoShotThis);
            effects.CameraShake(0.07f);
        }
    }
}
