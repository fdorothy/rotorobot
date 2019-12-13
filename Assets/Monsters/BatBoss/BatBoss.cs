using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BatBoss : MonoBehaviour
{
    Creature monster;
    Rigidbody2D rb;
    Animator anim;
    public float facing = 1.0f;
    public float maxSpeed;

    [SerializeField] public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        monster = GetComponentInChildren<Creature>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        StartCoroutine(MoveRoutine());
    }

    public void FixedUpdate()
    {
        if (monster.stunned) return;
        if (monster.IsDead())
        {
            rb.velocity = Vector2.zero;
        }
    }

    public IEnumerator MoveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            if (wallCheck()) {
                turnAround();
            }
            rb.velocity = new Vector3(facing * maxSpeed, 0, 0.0f);
        }
    }

    public void turnAround()
    {
        Debug.Log("Turning around");
        facing *= -1.0f;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public bool wallCheck()
    {
        Vector3 v = new Vector3(facing, 0.0f, 0.0f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, v, 3.0f, whatIsGround);
        bool wall = (hit.collider != null);
        return wall;
    }
}
