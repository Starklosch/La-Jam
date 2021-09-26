using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 direction;

    public event EventHandler<CollisionEventArgs> Collision;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(direction, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collision?.Invoke(this, new CollisionEventArgs(collision));
    }

    public class CollisionEventArgs : EventArgs
    {
        public Collision Collision { get; private set; }

        public CollisionEventArgs(Collision collision)
        {
            Collision = collision;
        }
    }
}
