using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class Player : MonoBehaviour
{
    private PlatformerCharacter2D m_Character;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 4f;
    private Rigidbody2D m_RigidBody;
    protected Animator m_Animator;
    private bool m_Jump = false;

    // Start is called before the first frame update
    void Start()
    {
        m_Character = GetComponent<PlatformerCharacter2D>();
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Jump)
        {
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }
    }

    void FixedUpdate()
    {
        // Read the inputs.
        float h = CrossPlatformInputManager.GetAxis("Horizontal");

        float multiplier = 1.0f;
        if (m_RigidBody.velocity.y < 0)
        {
            multiplier = fallMultiplier;
        }
        else if (m_RigidBody.velocity.y > 0 && !CrossPlatformInputManager.GetButton("Jump"))
        {
            multiplier = lowJumpMultiplier;
        }
        m_RigidBody.AddForce(Vector2.up * Physics2D.gravity.y * (multiplier - 1.0f));

        // Pass all parameters to the character control script.
        m_Character.Move(h, false, m_Jump);
        m_Jump = false;

    }
}
