using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour
{
    public GameObject prefabFireBall;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireBall(PlayerController controller)
    {
        var fb = Instantiate(prefabFireBall, transform.position, Quaternion.identity).GetComponent<Projectile>();

        fb.direction = controller.playerCamera.transform.forward;

    }
}
