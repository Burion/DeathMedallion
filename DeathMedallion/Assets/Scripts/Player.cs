using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit {

    float jumptimer;
    float currentjumptime;

    void Start () {

        base.Start();
        base.__init__(4f, 4);

        jumptimer = 1f;
        Damage = 1;

	}
	

	void Update () {
        
        CheckMove();
        UpdateAnimator();

        for (int i = 0; i < 20; i++)
        {
            if (Input.GetKeyDown("joystick 1 button " + i))
            {
                print("joystick 1 button " + i);
            }
        }
    }

    void CheckMove()
    {
        if (Input.GetAxisRaw("Horizontal") == -1)
        {
            Move(-1, speed);
        }
        else if (Input.GetAxisRaw("Horizontal") == 1)
        {
            Move(1, speed);
        }
        else

        rb.velocity = new Vector2(0, rb.velocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            Jump(400);
        }
        
        if (Input.GetButtonDown("Attack"))
        {
            anim.SetTrigger("attack");
        }
        
            
 
    }
    void UpdateAnimator()
    {
        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetBool("grounded", grounded);
    }
}
