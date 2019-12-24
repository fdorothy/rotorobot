using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotoship : MonoBehaviour
{
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Warp()
    {
        anim.SetBool("warp", true);
    }

    public void Idle()
    {
        anim.SetBool("warp", false);
    }
}
