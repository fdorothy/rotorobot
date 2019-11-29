using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDamage : MonoBehaviour
{
    public string enemyTag;

    public int damage = 1;

    void OnTriggerStay2D(Collider2D collider2D) {
        if (collider2D.tag == enemyTag) {
            Creature c = collider2D.GetComponent<Creature>();
            Vector2 dir = c.transform.position - transform.position;
            c.Hit(damage, (dir.normalized + Vector2.up) * 15.0f);
        }
    }
}
