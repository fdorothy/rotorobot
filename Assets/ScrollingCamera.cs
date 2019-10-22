using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingCamera : MonoBehaviour
{

    public float maxDistanceX = 1.5f;
    public float maxDistanceY = 1.5f;
    public Transform player;
    public Vector3 target;
    public bool locked = false;
    public float speed = 1.0f;
    public float dx = 0.0f;

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
        dx = player.transform.position.x - transform.position.x;
        float dy = player.transform.position.y - transform.position.y;
        dx = deltaWindow(dx, maxDistanceX);
        dy = deltaWindow(dy, maxDistanceY);
        Vector3 dv = new Vector3(dx, dy, 0.0f);
        transform.position += dv;
    }

    float deltaWindow(float t, float max) {
        if (Mathf.Abs(t) > max) {
            if (t > 0.0f) {
                return t - max;
            } else {
                return t + max; 
            }
        }
        return 0.0f;
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
