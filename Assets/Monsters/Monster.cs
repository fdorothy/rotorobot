using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Monster : MonoBehaviour
{
    public int hitpoints = 1;
    public SpriteRenderer spriteRenderer;
    protected bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = this.transform.GetComponentInChildren<SpriteRenderer>();
    }

    public void Hit(int damage)
    {
        if (dead)
        {
            return;
        }
        hitpoints -= damage;
        if (hitpoints <= 0)
        {
            dead = true;
            StartCoroutine(FlashingDeath());
        }
    }

    public bool IsDead() {return hitpoints <= 0;}

    public IEnumerator FlashingDeath()
    {
        for (int i = 0; i < 5; i++)
        {
            spriteRenderer.color = Color.clear;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(this.gameObject);

    }
}
