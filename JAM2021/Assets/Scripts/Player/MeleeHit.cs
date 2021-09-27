using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHit : MonoBehaviour
{
    public Weapons weaponsComponent;

    private void OnTriggerEnter(Collider other)
    {
        Enemy e = other.GetComponent<Enemy>();
        if (e)
        {
            e.TakeDamage(1);
            weaponsComponent.UseDurability();
        }
    }
}
