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
    public bool destroyOnKilled = true;

    protected Material material;

    public UnityEvent OnHit;
    public UnityEvent OnDying;
    public UnityEvent OnDead;

    public float invulnerableTime = 0.5f;
    public bool invulnerable = false;

    public bool stunned = false;

    public float stunTime = 0.25f;

    public Rigidbody2D rigidbody2d;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = this.transform.GetComponentInChildren<Rigidbody2D>();
        spriteRenderer = this.transform.GetComponentInChildren<SpriteRenderer>();
        material = spriteRenderer.material;
    }

    public void Hit(int damage, Vector2 dir)
    {
        if (OnHit != null)
            OnHit.Invoke();
        SFXController.PlayClip(SFXClipName.MONSTERHIT);
        rigidbody2d.AddForce(dir, ForceMode2D.Impulse);
        StartCoroutine(Stun());
        if (dead || invulnerable)
            return;
        hitpoints -= damage;
        if (hitpoints <= 0)
        {
            Kill();
        }
        else
        {
            StartCoroutine(Pain());
        }
    }

    public void Kill()
    {
        hitpoints = 0;
        if (OnDying != null)
            OnDying.Invoke();
        dead = true;
        StartCoroutine(FlashingDeath());
    }

    public bool IsDead() { return hitpoints <= 0; }

    public IEnumerator Pain()
    {
        Shake(0.2f);
        float t = 0.0f;
        float dt = 0.1f;
        invulnerable = true;
        while (t < invulnerableTime)
        {
            spriteRenderer.color = Color.white;
            material.SetFloat("SolidColor", 1.0f);
            yield return new WaitForSeconds(dt);
            spriteRenderer.color = Color.white;
            material.SetFloat("SolidColor", 0.0f);
            yield return new WaitForSeconds(dt);
            t += 2 * dt;
        }
        invulnerable = false;
    }

    public IEnumerator Stun()
    {
        stunned = true;
        yield return new WaitForSeconds(stunTime);
        stunned = false;
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
        if (OnDead != null)
            OnDead.Invoke();
        if (destroyOnKilled)
        {
            Destroy(this.gameObject);
        }
    }

    public void Shake(float duration)
    {
        spriteRenderer.transform.DOShakePosition(duration, new Vector3(0.2f, 0.0f, 0.0f), 20, 90, false, true);
    }
}
