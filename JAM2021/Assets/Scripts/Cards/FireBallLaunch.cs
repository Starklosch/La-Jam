using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallLaunch : MonoBehaviour
{
    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = new Ray(transform.position, Camera.main.transform.forward);
            Physics.Raycast(ray, out var hitInfo);

            var obj = Instantiate(prefab, hitInfo.point + Vector3.up * 10, Quaternion.identity);
        }
    }
}
