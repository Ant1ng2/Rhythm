using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartHitController : HitController
{
    public bool hit = false;

    void OnTriggerEnter2D(Collider2D collider)
    {
        hit = true;
        Destroy(collider.gameObject);
    }
}
