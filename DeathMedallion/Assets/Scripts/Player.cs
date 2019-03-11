using Anima2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit {

    float jumptimer;
    float currentjumptime;
    [SerializeField] SpriteMeshInstance eyes;
    [SerializeField] SpriteRenderer sword;
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
    public override void GotHit()
    {
        base.GotHit();
        StartCoroutine(StayUnhittable(2f));
    }
    void UpdateAnimator()
    {
        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetBool("grounded", isGrounded);
    }

    #region Corutines
    IEnumerator StayUnhittable(float time)
    {
        float interval = 0.3f;
        var body = transform.Find("Model").Find("Sprite Meshes");
        List<SpriteMeshInstance> bodyparts = new List<SpriteMeshInstance>();
        SpriteMeshInstance[] bodyParts = body.GetComponentsInChildren<SpriteMeshInstance>();
        foreach(SpriteMeshInstance part in bodyParts)
        {
            bodyparts.Add(part);
        }
        bodyparts.Add(eyes);
        if (bodyParts == null) throw new System.NullReferenceException("Bounds are Empty");
        while (time > 0)
        {

            foreach(SpriteMeshInstance part in bodyparts)
            {
                part.color = new Color32(255, 255, 255, 100);
                sword.color = new Color32(255, 255, 255, 100);
            }
            yield return new WaitForSeconds(interval);

            foreach (SpriteMeshInstance part in bodyparts)
            {
                part.color = Color.white;
                sword.color = Color.white;
            }
            yield return new WaitForSeconds(interval);
            time -= interval * 2;
        }
    }
    #endregion
}
