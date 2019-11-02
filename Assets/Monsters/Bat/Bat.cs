using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bat : MonoBehaviour
{
    Monster monster;
    Rigidbody2D rb;
    Animator anim;
    public float facing = 1.0f;

    public Transform groundCheck;
    public Transform wallCheck;

    public float flapTime;
    public float waitTime;
    public float turnTime;
    public Vector3 flapForce;
    public float maxSpeed;

    [SerializeField] public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        monster = GetComponentInChildren<Monster>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        StartCoroutine(FlapRoutine());
        StartCoroutine(MoveRoutine());
    }

    public void FixedUpdate()
    {
        if (monster.IsDead())
        {
            rb.velocity = Vector2.zero;
        }
    }

    public IEnumerator FlapRoutine()
    {
        bool flapUp = false;
        while (true)
        {
            if (flapUp)
            {
                flapUp = !ceilingCheck();
                for (int i = 0; i < 3; i++)
                {
                    if (rb.velocity.y < maxSpeed)
                    {
                        Flap();
                        yield return new WaitForSeconds(flapTime);
                    }
                }
            }
            else
            {
                flapUp = floorCheck();
                rb.AddForce(flapForce / 2.0f);

                for (int i = 0; i < 3; i++)
                {
                    if (rb.velocity.y < -maxSpeed)
                    {
                        Flap();
                        yield return new WaitForSeconds(flapTime);
                    }
                }
            }
            yield return new WaitForSeconds(waitTime);
        }
    }

    public IEnumerator MoveRoutine() {
        while (true) {
            rb.velocity = new Vector3(1.0f, rb.velocity.y, 0.0f);
            yield return new WaitForSeconds(4.0f);
            turnAround();
            rb.velocity = new Vector3(-1.0f, rb.velocity.y, 0.0f);
            yield return new WaitForSeconds(4.0f);
            turnAround();
        }
    }

    public void Flap()
    {
        Debug.Log("Flap");
        rb.AddForce(flapForce);
        anim.SetTrigger("Flap");
    }

    public void turnAround()
    {
        facing *= -1.0f;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public bool floorCheck()
    {
        Vector3 v = new Vector3(0.0f, -1.0f, 0.0f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (new Vector3(0.0f, 0.5f)), v, 3.0f, whatIsGround);
        bool floor = (hit.collider != null);
        return floor;
    }
    public bool ceilingCheck()
    {
        Vector3 v = new Vector3(0.0f, 1.0f, 0.0f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (new Vector3(0.0f, 0.5f)), v, 2.0f, whatIsGround);
        bool floor = (hit.collider != null);
        return floor;
    }
}
