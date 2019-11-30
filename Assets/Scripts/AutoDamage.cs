using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDamage : MonoBehaviour
{
    public string enemyTag;

    public int damage = 1;

    public bool autoKill = false;

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        DoAutoDamage(collider2D);
    }

    void OnTriggerStay2D(Collider2D collider2D)
    {
        DoAutoDamage(collider2D);
    }

    void DoAutoDamage(Collider2D collider2D)
    {
        if (collider2D.tag == enemyTag)
        {
            Creature c = collider2D.GetComponent<Creature>();
            Vector2 dir = c.transform.position - transform.position;
            if (autoKill)
            {
                c.Kill();
            }
            else
            {
                c.Hit(damage, (dir.normalized + Vector2.up) * 5.0f);
            }
        }

    }
}
