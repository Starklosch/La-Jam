using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHit : MonoBehaviour
{
    public Weapons weaponsComponent;

    private void OnTriggerEnter(Collider other)
    {
        Enemy e = other.attachedRigidbody.GetComponent<Enemy>();
        if (e)
        {
            e.TakeDamage((int)weaponsComponent.GetCurrentDamage());
            weaponsComponent.UseDurability();
        }
    }
}
