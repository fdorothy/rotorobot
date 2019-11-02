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

    public bool lockX = false;
    public bool lockY = false;

    public UnityEngine.Tilemaps.Tilemap map;

    Vector3 min, max;

    float width, height;

    // Use this for initialization
    void Start()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player").transform;
        target = player.transform.position;

        min = map.LocalToWorld(map.localBounds.min);
        max = map.LocalToWorld(map.localBounds.max);

        Camera cam = GetComponent<Camera>();
        height = cam.orthographicSize;
        width = ((float)cam.pixelWidth / cam.pixelHeight) * height;
        Debug.Log("width and height: " + height.ToString() + ", " + width.ToString());
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
        Vector3 dv = new Vector3(dx * (lockX ? 0.0f : 1.0f), dy * (lockY ? 0.0f : 1.0f), 0.0f);
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x + dv.x, min.x + width, max.x - width),
            Mathf.Clamp(transform.position.y + dv.y, min.y + height, max.y - height),
            transform.position.z
        );
    }

    float deltaWindow(float t, float max)
    {
        if (Mathf.Abs(t) > max)
        {
            if (t > 0.0f)
            {
                return t - max;
            }
            else
            {
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
