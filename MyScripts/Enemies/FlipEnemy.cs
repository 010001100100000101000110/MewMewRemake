using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlipEnemy : MonoBehaviour
{
    Vector3 originalScale;
    Vector3 normal;
    Vector3 flip;
    EnemyHelper helper;
    void Start()
    {
        helper = GetComponent<EnemyHelper>();
        originalScale = transform.localScale;
        normal = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        flip = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
    }

    void Update()
    {
        EnemyFlip(helper.Stats.Type);
    }
    void EnemyFlip(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Pachycephalosaurus:
                if (helper.Animator.GetCurrentAnimatorStateInfo(0).IsName("Charge")) FlipTowardPlayer();
                else FlipTowardDirection();
                break;

            case EnemyType.Ankylosaurus:
                if (helper.Animator.GetBool("isShooting")) FlipTowardPlayer();
                else FlipTowardDirection();
                break;

            default:
                FlipTowardDirection();
                break;
        }
    }
    void FlipTowardPlayer()
    {
        if (gameObject.transform.position.x > helper.Player.position.x) transform.localScale = normal;
        else transform.localScale = flip;
    }

    void FlipTowardDirection()
    {
        if (helper.Agent.velocity.x < 0) transform.localScale = normal;
        if (helper.Agent.velocity.x > 0) transform.localScale = flip;
    }
}
