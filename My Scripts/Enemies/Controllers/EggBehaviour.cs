using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBehaviour : MonoBehaviour
{
    Animator anim;
    [SerializeField] GameObject baby;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void DefineBaby(GameObject go)
    {
        baby = go;
    }
    public void StartHatching()
    {
        anim.SetBool("IsCracking", true);
    }
    //animation event
    void SpawnBaby()
    {
        baby.SetActive(true);
    }
    void DestroyEgg()
    {
        Destroy(gameObject);
    }
}

