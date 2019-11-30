using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public string enemyTag;

    public int damage = 1;

    public float force = 10.0f;

    void OnTriggerStay2D(Collider2D collider2D) {
        if (collider2D.gameObject.tag == enemyTag) {
            Creature c = collider2D.gameObject.GetComponent<Creature>();
            c.Hit(damage, force * Vector2.up);
        }
    }
}
