using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindSpider : Enemy
{
    

    private void Start()
    {
        base.Start();
        anim = GetComponentInChildren<Animator>();
        base.Start();
        Health = 4;
    }
    private void Update()
    {
        UpdateAnimator();
    }
    void UpdateAnimator()
    {
        if (mng.Player.transform.position.x < transform.position.x)
        {
            anim.SetBool("isLeft", true);
        }
        else
            anim.SetBool("isLeft", false);
    }
}
