using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyHandler : MonoBehaviour
{
    [SerializeField] GameObject egg;
    [SerializeField] GameObject baby;

    private void Awake()
    {
        egg.GetComponent<EggBehaviour>().DefineBaby(baby);
    }
    private void Update()
    {
        if (gameObject.transform.childCount == 0) Destroy(gameObject);
    }
}
