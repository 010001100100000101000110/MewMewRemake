using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriceratopsFireTrail : MonoBehaviour
{
    FireTrailPool fireTrails;
    Vector2 lastFireTrail;
    [SerializeField] Transform fireTrailPlacement;

    void Start()
    {
        fireTrails = FindObjectOfType<FireTrailPool>();
    }

    void Update()
    {
        FireTrail();
    }

    void FireTrail()
    {
        if (Vector3.Distance(lastFireTrail, fireTrailPlacement.position) > 1)
        {
            fireTrails.GetFireTrail(fireTrailPlacement.position);
            lastFireTrail = fireTrailPlacement.position;
        }
    }
}
