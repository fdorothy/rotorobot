using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    public Sprite explosionSprite;
    public Sprite shootingSprite;
    public float speed;
    public Vector3 dir;

    public ParticleSystem trailSystem;
    public ParticleSystem blastSystem;
    public ParticleSystem chargingSystem;
    public SpriteRenderer sprite;

    protected bool blownUp = false;

    [SerializeField] public LayerMask layerMask;
    protected bool shooting = false;
    public float charge = 0.0f;

    public Sprite[] beamStages;
    protected DG.Tweening.Tween fader;

    // Start is called before the first frame update
    void Start()
    {
        shake();
    }

    public void SetDir(Vector2 dir, bool charging = false)
    {
        this.dir = dir.normalized;
        if (dir.x < 0.0f)
        {
            this.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }

        if (charging)
        {
            sprite.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
            fader = this.sprite.DOFade(1.0f, 3.0f);
            chargingSystem.Play();
        }
    }
    public void Shoot(Vector2 dir)
    {
        this.transform.parent = null;
        this.dir = dir.normalized;
        shooting = true;
        Invoke("Kill", 3.0f);
        trailSystem.Play();
        if (fader != null)
        {
            fader.Kill();
            fader.OnComplete(() => sprite.color = Color.white);
        }
        sprite.sprite = shootingSprite;
        chargingSystem.Stop();
    }

    public void SetCharge(float value)
    {
        charge = value;
    }

    public void shake()
    {
        this.sprite.transform.DOShakePosition(0.1f, charge / 7.5f).OnComplete(() =>
        {
            shake();
        });
    }

    private void Update()
    {
        if (!shooting)
            return;
        if (blownUp)
        {
            sprite.sprite = null;
            return;
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, speed * Time.deltaTime, layerMask);
        if (hit.collider != null)
        {
            BlowUp(hit.point);

            // did we hit an enemy?
            Creature m = hit.transform.GetComponent<Creature>();
            if (m)
            {
                m.Hit(1);
            }
        }
        else
        {
            transform.Translate(dir * speed * Time.deltaTime);
        }
    }

    void BlowUp(Vector2 position)
    {
        blownUp = true;
        sprite.sprite = explosionSprite;
        trailSystem.Stop();
        blastSystem.Play();
        this.transform.position = position;
        Invoke("Kill", 0.5f);
    }

    void Kill()
    {
        Destroy(this.gameObject);
    }
}
