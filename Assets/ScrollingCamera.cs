using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScrollingCamera : MonoBehaviour
{

    public float maxDistance = 1.5f;
    public Transform player;
    public Vector3 target;
    public bool locked = false;

    // Use this for initialization
    void Start()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player").transform;
        target = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (locked)
            return;
        float dx = player.transform.position.x - transform.position.x;
        float dy = 0.0f;
        if (dx * dx + dy * dy > maxDistance * maxDistance)
        {
            reorient();
        }

        dx = target.x - transform.position.x;
        dy = 0.0f;
        float d = dx * dx + dy * dy;
        if (d > 0.5f)
        {
            transform.position += (new Vector3(dx, dy, 0.0f)) * Time.deltaTime;
        }
    }

    void reorient()
    {
        target = player.transform.position;
    }

    void SetLocked(bool val)
    {
        locked = val;
    }
}
