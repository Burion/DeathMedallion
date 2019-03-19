using Anima2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Unit {

    public bool godMode = false;
    float jumptimer;
    float currentjumptime;
    bool canHit = true;
    [SerializeField] SpriteMeshInstance eyes;
    [SerializeField] SpriteRenderer sword;
    public Transform healthTransform;


    void Start () {

        base.Start();
        base.__init__(4f, 4);
        Health = 5;
        OnHealthChange += new Dying(SetHearthCount);
        jumptimer = 1f;
        Damage = 1;
        hitable = true;

	}

    public override void DeathState()
    {
        if (Health <= 0)
        {
            base.DeathState();
            Info.IsGameOn = false;
            mng.OnDie();
        }
    }
	void Update () {

        if (Info.IsGameOn)
        {
            GodMode();
            CheckMove();
            UpdateAnimator();
        }

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
        ////
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Health--;
        }
        ///
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
            if ((doubleJmp == true && !isGrounded) || isGrounded)
            {
                if(!isGrounded)
                doubleJmp = false;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                Jump(400);
            }
        }
        
        if (Input.GetButtonDown("Attack"))
        {
            if (!canHit) return;
            anim.SetTrigger("attack");
            StartCoroutine(BlockHit(0.3f)); // TODO decision about time block
        }
         
    }
    public override void GotHit()
    {
        if (!hitable) return;
        base.GotHit();
        StartCoroutine(StayUnhittable(2f));
    }
    void UpdateAnimator()
    {
        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetBool("grounded", isGrounded);
    }

    public void SetGodMode(bool choice)
    {
        godMode = choice;   
    }
    void GodMode()
    {
        if (godMode)
        {
            doubleJmp = true;
            Health = int.MaxValue;
        }
    }
    void SetHearthCount()
    {
        Debug.Log(Health);
        Color32 filled = new Color32(255, 255, 255, 255);
        Color32 empty = new Color32(87, 87, 87, 53);
        int toSet;
        if (Health > healthTransform.childCount) toSet = healthTransform.childCount;
        else
            toSet = Health;
        for(int i = 0; i < healthTransform.childCount; i++)
        {
            healthTransform.GetChild(i).GetComponent<Image>().color = i < toSet ? filled : empty;
        }
    }
    #region Corutines
    IEnumerator StayUnhittable(float time)
    {
        hitable = false;
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
        hitable = true;
    }
    IEnumerator BlockHit(float time)
    {
        canHit = false;
        yield return new WaitForSeconds(time);
        canHit = true;
    }
    #endregion
}
