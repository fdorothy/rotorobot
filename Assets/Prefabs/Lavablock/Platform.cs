using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject target = null;
    public Vector3 offset;
    void Start()
    {
        target = null;
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            target = col.gameObject;
            offset = target.transform.position - transform.position;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        target = null;
    }
    public void AdjustTarget(Vector3 dv)
    {
        if (target != null)
        {
            target.transform.position += dv;
        }
    }
}
