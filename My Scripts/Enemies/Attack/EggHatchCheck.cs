using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggHatchCheck : MonoBehaviour
{
    [SerializeField] GameObject[] eggs;
    [SerializeField] GameObject triceratops;
    bool hasHatched;


    private void Update()
    {
        if (gameObject.transform.childCount <= 0) Destroy(gameObject);
        if (!hasHatched)
        {
            if (triceratops != null)
            {
                if (Vector3.Distance(gameObject.transform.position, triceratops.transform.position) > 4) Hatch();
            }
            else Hatch();
        }        
    }

    void Hatch()
    {
        hasHatched = true;
        for (int i = 0; i < eggs.Length; i++)
        {
            eggs[i].GetComponentInChildren<EggBehaviour>().StartHatching();
        }
    }

    public void SetTriceratops(GameObject go)
    {
        triceratops = go;
    }
}
