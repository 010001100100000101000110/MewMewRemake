using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriceratopsEggLay : MonoBehaviour
{
    TriceratopsController controller;
    EnemyHelper helper;

    [SerializeField] GameObject eggsAndBabies;
    [SerializeField] Transform eggsAndBabiesSpawnpoint;

    public bool IsLayingEggs { get; private set; }
    float attackCooldown;
    float nextAttack;
    int eggsLaid;
    [SerializeField] int maxEggLays;

    void Start()
    {
        helper = GetComponent<EnemyHelper>();
        attackCooldown = helper.Stats.AttackCooldown;
        controller = GetComponent<TriceratopsController>();
    }

    public void EggLayUpdate()
    {
        if (!helper.TGManager.TopGunning)
        {
            if (eggsLaid >= maxEggLays) controller.ChangeState(controller.IdleState);

            if (Time.time < nextAttack) return;
            if (controller.InRange()) LayEggs();
        }
        else controller.ChangeState(controller.IdleState);
    }
    public void LayEggs()
    {
        IsLayingEggs = true;        
        helper.Animator.SetTrigger("LayEggs");
        nextAttack = Time.time + attackCooldown;
    }

    //animation event
    void SpawnEggs()
    {
        eggsLaid++;
        helper.Animator.ResetTrigger("LayEggs");
        GameObject go = Instantiate(eggsAndBabies, eggsAndBabiesSpawnpoint.position, Quaternion.identity);
        go.GetComponent<EggHatchCheck>().SetTriceratops(gameObject);

        EnemyHelper[] helpers = go.GetComponentsInChildren<EnemyHelper>();
        for (int i = 0; i < helpers.Length; i++)
        {
            helpers[i].Initialize(helper.Player, helper.Manager, helper.TGManager, 1);
            helper.Manager.AddEnemyToActive(helpers[i].gameObject);
            helpers[i].gameObject.SetActive(false);
        }
        IsLayingEggs = false;
    }

    public void ResetEggLay()
    {
        eggsLaid = 0;
        IsLayingEggs = false;
    }
}
