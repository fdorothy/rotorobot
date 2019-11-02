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

    public Beam beamPrefab;
    public Beam beamObject;
    public Transform pistolMount;

    protected bool shooting = false;

    protected bool charging = false;

    protected bool inChargeZone = false;

    protected bool requestShot = false;
    public float weaponCharge = 0.0f;

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

        bool fire1 = CrossPlatformInputManager.GetButtonDown("Fire1");
        if (fire1 || (requestShot && !shooting))
        {
            if (!charging && !shooting)
            {
                beamObject = Instantiate(beamPrefab);
                beamObject.SetDir(new Vector2(m_Character.m_FacingRight ? 1.0f : -1.0f, 0.0f), inChargeZone);
                beamObject.transform.position = this.pistolMount.transform.position;
                beamObject.transform.parent = this.pistolMount;
                if (inChargeZone)
                {
                    charging = true;
                    weaponCharge = 0.0f;
                }
                else
                {
                    charging = false;
                    Fire();
                    weaponCharge = 0.0f;
                }
                requestShot = false;
            }
            else
            {
                if (fire1)
                {
                    Debug.Log("requesting extra shot...");
                    requestShot = true;
                }
            }
        }
        if (charging && beamObject)
        {
            m_Animator.SetBool("Shooting", true);
            weaponCharge += Time.deltaTime;
            const float maxCharge = 1.5f;
            if (weaponCharge > maxCharge)
            {
                weaponCharge = maxCharge;
            }
            beamObject.SetCharge(weaponCharge / maxCharge);
            if (CrossPlatformInputManager.GetButtonUp("Fire1"))
            {
                Debug.Log("let go");
                charging = false;
                Fire();
                weaponCharge = 0.0f;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Charger")
        {
            inChargeZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Charger")
        {
            inChargeZone = false;
        }
    }

    void Fire()
    {
        if (shooting)
            return;
        shooting = true;
        m_Animator.SetBool("Shooting", true);
        beamObject.Shoot(new Vector2(m_Character.m_FacingRight ? 1.0f : -1.0f, 0.0f));
        beamObject = null;
        Invoke("ResetGun", 0.25f);
    }

    void ResetGun()
    {
        shooting = false;
        m_Animator.SetBool("Shooting", false);
    }
}
