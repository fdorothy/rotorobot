using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Creature : MonoBehaviour
{
    public int hitpoints = 1;
    public SpriteRenderer spriteRenderer;
    protected bool dead = false;

    protected Material material;

    public UnityEvent OnHit;
    public UnityEvent OnDying;
    public UnityEvent OnDead;

    // Start is called before the first frame update
    void Start()
    {
        OnHit = new UnityEvent();
        OnDying = new UnityEvent();
        OnDead = new UnityEvent();
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
        OnHit.Invoke();
        if (hitpoints <= 0)
        {
            OnDying.Invoke();
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
        OnDead.Invoke();
        Destroy(this.gameObject);

    }
    
    public void Shake(float duration) {
        spriteRenderer.transform.DOShakePosition(duration, new Vector3(0.2f, 0.0f, 0.0f), 20, 90, false, true);
    }
}
