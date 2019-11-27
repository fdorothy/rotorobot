using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject CreaturePrefab;
    public int spawnCount = 1;
    public float spawnDelay = 1.0f;

    public void Spawn()
    {
        if (spawnCount == 0) {
            Destroy(this.gameObject);
            return;
        }
        if (spawnCount > 0)
            spawnCount--;
        Debug.Log("got this far");
        GameObject c = Instantiate(CreaturePrefab) as GameObject;
        c.transform.position = this.transform.position;
    }

    public void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Player") {
            Debug.Log("spawning");
            Spawn();
            Debug.Log("done");
        }
    }
}
