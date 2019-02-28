using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{

}

public class Unit : MonoBehaviour {

    public delegate void Dying();
    public Animator anim;
    public event Dying OnHealthChange;
    public Rigidbody2D rb { get; set; }
    int health;
    public bool grounded;
    public bool ableToJump;
    int damage;
    public int Damage
    {
        get
        {
            return health;
        }
        set
        {
            damage = value;
        }
    }
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            if (value != health)
            {
                health = value;
                OnHealthChange();
            }
                
        }
    }
    public int CurrentState { get; set; }

    void __init__()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        OnHealthChange += new Dying(DeathState);
        rb = gameObject.GetComponent<Rigidbody2D>();
        ableToJump = true;
    }
    void DeathState()
    {
        if (Health <= 0)
        {
            anim.Play("die");
            StateDeath();
            List<GameObject> deadGameObjects = new List<GameObject>();
        }
    }
    public void StateDeath()
    {
        SwitchState((int)States.death);
    }
    public void SwitchState(int stateIndex)
    {
        CurrentState = stateIndex;
    }
    public void Start ()
    {
        __init__();
	}
	

    public void Move(int dir, float speed)
    {
        rb.velocity = new Vector2(dir*speed, rb.velocity.y);
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x) * dir, transform.localScale.y, transform.localScale.z);

    }
    public void Jump(float power)
    {
        rb.AddForce(new Vector2(0, 1 * power));
    }
    public virtual void GotHit(int dir)
    {
        anim.Play("gethit");
        Health -= 1;
        GetRecoil(dir);
    }
    public void GetRecoil(float dir)
    {
        rb.AddForce(new Vector2(100 * dir, 50));
    }


}
