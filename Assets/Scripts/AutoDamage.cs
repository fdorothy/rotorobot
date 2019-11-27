using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDamage : MonoBehaviour
{
    public string enemyTag;

    public int damage = 1;

    void OnTriggerEnter2D(Collider2D collider2D) {
        if (collider2D.tag == enemyTag) {
            Creature c = collider2D.GetComponent<Creature>();
            c.Hit(damage);
        }
    }
}
