using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;

    public int enemyCount;

    public float delay;

    public float delayBetweenEnemy;

    float nextTime;

    public Vector3 size;
    public Vector3 center;

    int spawnedEnemies = 0;

    // Start is called before the first frame update
    void Start()
    {
        nextTime = delay + Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(spawnedEnemies < enemyCount);
        if (spawnedEnemies < enemyCount && nextTime < Time.time)
        {
            float x = size.x / 2;
            float y = size.y / 2;
            float z = size.z / 2;
            x = Random.Range(x, x);
            y = Random.Range(y, y);
            z = Random.Range(z, z);
            Instantiate(enemy, center + transform.position + new Vector3(x, y, z), Quaternion.identity);

            nextTime = Time.time + delayBetweenEnemy;

            spawnedEnemies++;
        }
    }

    private void OnDrawGizmos()
    {
        var color = Gizmos.color;
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube(transform.position + center, size);

        Gizmos.color = color;
    }
}
