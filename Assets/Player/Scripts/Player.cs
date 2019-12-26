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
    public bool paused = false;

    public Bullet beamPrefab;
    public Bullet beamObject;
    public Transform pistolMount;

    protected bool shooting = false;

    protected bool charging = false;

    protected bool inChargeZone = false;

    protected bool requestShot = false;
    public float weaponCharge = 0.0f;

    protected bool canLaunch;
    protected bool launching;
    protected Vector2 launchDir;

    public ParticleSystem jetParticles;

    protected Creature creature;

    // Start is called before the first frame update
    void Start()
    {
        m_Character = GetComponent<PlatformerCharacter2D>();
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        creature = GetComponent<Creature>();
        canLaunch = false;
        launching = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (creature.hitpoints == 0 || paused) {
            return;
        }
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        bool jump = CrossPlatformInputManager.GetButtonDown("Jump");
        if (jump)
        {
            if (m_Character.isGrounded())
            {
                m_Jump = true;
                canLaunch = true;
            }
            else if (canLaunch)
            {
                launchDir = new Vector2(h, v);
                if (launchDir.magnitude < 0.1f) {
                    // launch up by default
                    launchDir = Vector2.up;
                }
                if (launchDir.magnitude > 0.1f)
                {
                    launching = true;
                    canLaunch = false;
                    StartCoroutine(DoLaunch());
                }
            }
        }
    }

    IEnumerator DoLaunch()
    {
        jetParticles.Play();
        m_Animator.SetBool("Launching", true);
        float gravity = m_RigidBody.gravityScale;
        m_RigidBody.gravityScale = 0.0f;
        yield return new WaitForSeconds(0.1f);
        m_RigidBody.velocity = launchDir.normalized * 20.0f;
        yield return new WaitForSeconds(0.5f);
        m_RigidBody.velocity = Vector2.zero;
        m_RigidBody.gravityScale = gravity;
        launching = false;
        canLaunch = false;
        m_Animator.SetBool("Launching", false);
        jetParticles.Stop();
    }

    void FixedUpdate()
    {
        if (creature.stunned) {
            return;
        }
        if (creature.hitpoints == 0) {
            this.m_RigidBody.velocity = Vector2.zero;
            return;
        }
        if (launching) return;

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

        if (paused) {
            h = 0.0f;
            m_Jump = false;
        }
        // Pass all parameters to the character control script.
        m_Character.Move(h, false, m_Jump);
        m_Jump = false;
        if (paused) return;
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
        SFXController.PlayClip(SFXClipName.SHOOT);
        beamObject = null;
        Invoke("ResetGun", 0.25f);
    }

    void ResetGun()
    {
        shooting = false;
        m_Animator.SetBool("Shooting", false);
    }

    public void OnHit() {
        SFXController.PlayClip(SFXClipName.PLAYERHIT);
    }

    public void OnDying()
    {
        SFXController.PlayClip(SFXClipName.PLAYERHIT);
    }

    public void OnDead() {
        Debug.Log("Changing scene to title");
        string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
}
