using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lavablock : MonoBehaviour
{
    Rigidbody2D rb;

    public float speed;

    public bool movingRight = false;
    [SerializeField] public LayerMask whatIsGround;

    public Transform leftChecker, rightChecker;
    public bool isVisible = false;

    public Vector3 lastPosition;

    public Platform platform;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isVisible = false;
    }

    void Update()
    {
        if (platform)
            platform.AdjustTarget(transform.position - lastPosition);
        lastPosition = this.transform.position;
    }

    void FixedUpdate()
    {
        if (!isVisible) return;
        rb.velocity = new Vector3(movingRight ? speed : -speed, rb.velocity.y, 0.0f);

        if (movingRight)
        {
            if (checkRight())
                movingRight = false;
        }
        else
        {
            if (checkLeft())
                movingRight = true;
        }
    }

    public bool checkLeft()
    {
        return checkGround(leftChecker.position, new Vector2(-1, 0), 0.2f);
    }
    public bool checkRight()
    {
        return checkGround(rightChecker.position, new Vector2(1, 0), 0.2f);
    }

    public bool checkGround(Vector2 origin, Vector2 dir, float distance)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, distance, whatIsGround);
        bool floor = (hit.collider != null);
        return floor;
    }

    public void OnBecameVisible()
    {
        Debug.Log("Woke up!");
        isVisible = true;
        rb.WakeUp();
    }
}
