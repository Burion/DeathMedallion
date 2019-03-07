using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{

}

public class Unit : MonoBehaviour {

    public Manager mng;
    public Vector2 target;
    public float speed;
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
    public SpeedManager spdmng;
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

    public virtual void __init__(float speed, int dmg)
    {
        mng = GameObject.Find("Manager").GetComponent<Manager>();
        this.speed = speed;
        Damage = dmg;
        anim = gameObject.GetComponentInChildren<Animator>();
        OnHealthChange += new Dying(DeathState);
        rb = gameObject.GetComponent<Rigidbody2D>();
        ableToJump = true;
        spdmng = new SpeedManager(speed);
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
        __init__(1.2f, 1);
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
    public void MovingTowardsTarget(Vector2 trgt)
    {
        if (transform.GetChild(0).position.x > trgt.x + 0.5f)
            Move(-1, speed);
        else
            Move(1, speed);
    }
    public void MovingTowardsTarget()
    {
        if (transform.Find("groundChecker").position.x > target.x + 0.5f)
            Move(-1, speed);
        else
            Move(1, speed);
    }
    public Vector2 GetNearest()
    {
        return mng.GetNearest(transform.Find("groundChecker").position);
    }
}
public class SpeedManager
{
    public float realSpeed;
    public SpeedManager(float realSpeed)
    {
        this.realSpeed = realSpeed;
    }

    public float ChangeSpeed(float delta)
    {
        float temp = realSpeed + delta;
        return temp;
    }
    public float SetSpeed(float speed)
    {
        return realSpeed = speed;
    }
    public float RevertSpeed()
    {
        return realSpeed;
    }
}