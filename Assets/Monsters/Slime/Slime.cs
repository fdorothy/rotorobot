using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Slime : MonoBehaviour
{
    protected Creature monster;
    Rigidbody2D rb;
    Animator anim;
    public float facing = 1.0f;

    public float slideForce = 1.0f;

    public Transform groundCheck;
    public Transform wallCheck;

    public float slideTime;
    public float waitTime;
    public float turnTime;
    public Transform shootPoint;
    public Transform bulletPrefab;

    [SerializeField] public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        monster = GetComponent<Creature>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        StartCoroutine(WalkRoutine());
    }

    public void FixedUpdate()
    {
        if (monster.IsDead())
        {
            rb.velocity = Vector2.zero;
            return;
        }
        if (!floorCheck())
        {
            if (!monster.stunned)
            {
                rb.velocity = new Vector2(0.0f, rb.velocity.y);
            }
        }
    }

    public IEnumerator WalkRoutine()
    {
        while (true)
        {
            if (safeToSlide())
            {
                if (!monster.stunned)
                {
                    anim.SetTrigger("Slide");
                    rb.velocity = new Vector2(slideForce * facing, 0.0f);
                }
                yield return new WaitForSeconds(slideTime);
                if (!monster.stunned)
                {
                    rb.velocity = new Vector2(0.0f, 0.0f);
                }
                yield return new WaitForSeconds(waitTime);
            }
            else
            {
                if (!monster.stunned)
                    turnAround();
                yield return new WaitForSeconds(turnTime);
            }
        }
    }

    public void turnAround()
    {
        facing *= -1.0f;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void shoot()
    {
        Transform bullet = Instantiate(bulletPrefab);
    }

    public bool safeToSlide()
    {
        bool ground = Physics2D.OverlapCircle(groundCheck.position, .1f, whatIsGround);
        Vector2 v = new Vector3(facing, 0.0f, 0.0f);
        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, v, 0.5f, whatIsGround);
        bool wall = (hit.collider != null);
        v = new Vector3(0.0f, -1.0f, 0.0f);
        hit = Physics2D.Raycast(wallCheck.position, v, 0.5f, whatIsGround);
        bool floor = (hit.collider != null);
        return (!wall && floor);
    }

    public bool floorCheck()
    {
        Vector3 v = new Vector3(0.0f, -1.0f, 0.0f);
        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, v, 0.5f, whatIsGround);
        bool floor = (hit.collider != null);
        return floor;
    }
}
