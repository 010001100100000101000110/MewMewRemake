using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaptorAttack : MonoBehaviour
{
    EnemyHelper helper;
    float attackCooldown;
    float nextAttack;
    bool isInRange;
    void Start()
    {
        helper = GetComponent<EnemyHelper>();
        attackCooldown = helper.Stats.AttackCooldown;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isInRange = false;
        helper.Animator.SetBool("DoAttack", false);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!helper.TGManager.TopGunning)
        {
            if (Time.time < nextAttack)
            {
                helper.Animator.SetBool("DoAttack", false);
                return;
            }
            isInRange = true;
            if (collision.gameObject == helper.Player.gameObject) helper.Animator.SetBool("DoAttack", true);
        }
        else
        {
            helper.Animator.SetBool("DoAttack", false);
        }
            
    }

    public void Attack()
    {
        if (isInRange) helper.Player.GetComponent<PlayerHealth>().TakeDamage(helper.Stats.Damage, this.gameObject);
        helper.Animator.SetBool("DoAttack", false);
        nextAttack = Time.time + attackCooldown;
    }
}
