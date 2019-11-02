using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Monster : MonoBehaviour
{
    public int hitpoints = 1;
    public SpriteRenderer spriteRenderer;
    protected bool dead = false;

    protected Material material;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = this.transform.GetComponentInChildren<SpriteRenderer>();
        material = spriteRenderer.material;
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
        else
        {
            StartCoroutine(Pain());
        }
    }

    public bool IsDead() { return hitpoints <= 0; }

    public IEnumerator Pain()
    {
        Shake(0.2f);
        //spriteRenderer.color = Color.red;
        material.SetFloat("SolidColor", 1.0f);
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
        material.SetFloat("SolidColor", 0.0f);
        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator FlashingDeath()
    {
        Shake(0.5f);
        for (int i = 0; i < 4; i++)
        {
            spriteRenderer.color = Color.red;
            material.SetFloat("SolidColor", 1.0f);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
            material.SetFloat("SolidColor", 0.0f);
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(this.gameObject);

    }
    
    public void Shake(float duration) {
        spriteRenderer.transform.DOShakePosition(duration, new Vector3(0.2f, 0.0f, 0.0f), 20, 90, false, true);
    }
}
