using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Anima2D;


public class Enemy: Unit
{
    public LayerMask layermask;
    public static System.Random r = new System.Random();
    List<State> states = new List<State>();
    public List<Vector2> bounds;
    public int enemyLevel;
    public event AlarmCons FoundPlayer;
    public delegate void AlarmCons();
    protected List<Vector2> Bounds;
    [SerializeField] public GameObject pair;
    [SerializeField] Transform parts;

    private void Awake()
    {
        if (enemyLevel > Info.CharmLevel)
        {
            SetVisability(false);
        }
        //Physics2D.IgnoreCollision(transform.Find("Collider").GetComponent<Collider2D>(), mng.Player.GetComponent<Collider2D>());
        Collider2D col = new Collider2D();
        Physics2D.IgnoreLayerCollision(10, 10);
        State patrolling = new State("patrolling", 1);
        State chasing = new State("chasing", 2);
        states.Add(patrolling);
        states.Add(chasing);
        try
        {
            FindState("patrolling");
        }
        catch (Exception e)
        {
            Debug.Log("State with this name doesn't exist");
        }
        currentState = (int)States.patrolling;

    }
    State FindState(string name)
    {
        State stateToReturn = null;
        foreach(State state in states)
        {
            if(state.name == name)
            {
                stateToReturn = state;
            }
        }
        if (stateToReturn == null)
        {
            throw new Exception("State doesn't exist");
        }
        return null;
    }
    void Start()
    {
        State patrolling = new State("patrolling", 1);
        State chasing = new State("chasing", 2);
        states.Add(patrolling);
        states.Add(chasing);
        Debug.Log("wow");
        try
        {
            FindState("patrollin");
        }
        catch (Exception e)
        {
            Debug.Log("State with this name doesn't exist");
        }
        __init__(1, 1);
    }

    public void Update()
    {
        UpdateAnimator();
        switch (currentState)
        {
            case (int)States.patrolling:

                Wandering();
                break;

            case (int)States.chasing:
                Chasing();
                break;

            case (int)States.combat:
                transform.localScale = new Vector2(mng.Player.transform.position.x > transform.position.x ? -Math.Abs(transform.localScale.x) : Math.Abs(transform.localScale.x), transform.localScale.y);
                speed = 0f;

                break;

            case (int)States.death:

                break;

        }
    }
    public virtual void StartChasing()
    {
        currentState = (int)States.chasing;
        anim.Play("walking");
        speed = spdmng.RevertSpeed();
        anim.SetTrigger("lostPlayer");
        anim.SetBool("combat", false);
        FoundPlayer();
    }
    public class State
    {
        public State(string name, float speed)
        {
            this.name = name;
            this.speed = speed;
        }
        public string name;
        public float speed;
    }

    #region Functions
    
    public void SetBounds(int range)
    {
        Bounds = mng.GetPlaneVertices(transform.GetChild(0).position, range);
    }
    public virtual void StartCombatMode()
    {
        currentState = (int)States.combat;
        anim.SetBool("combat", true);
    }
    public virtual void UpdateAnimator()
    {

    }
    public virtual void Chasing()
    {

    }
    public virtual void SetVisability(bool choice)
    {
        if (!parts) return;
        byte alpha = choice ? (byte)255 : (byte)100;
        if (parts)
        {
            foreach (Transform part in parts)
            {
                part.GetComponent<SpriteMeshInstance>().color = new Color32(255, 255, 255, alpha);
            }
        }
        GetComponent<Collider2D>().enabled = choice;
    }
    public virtual void Wandering()
    {
        FixWandering();
        if (IsOnThePoint())
        {
            if (speed > 0)
                StartCoroutine("StayWatch", 0.1f);
        }
        MovingTowardsTarget();

        Glance();
    }
    
    public bool IsInResponsiveZone()
    {
        foreach (Vector2 point in Bounds)
        {
            if (mng.GetNearest(gameObject.transform.GetChild(0).position) == point)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsOnThePoint()
    {
        return mng.GetNearest(transform.GetChild(0).position) == target;
    }

    public void GetRandomTargetFromBounds()
    {
        target = Bounds[r.Next(Bounds.Count)];
    }

    public void FixWandering()
    {
        bool needToCgange = true;
        foreach (Vector2 trgt in Bounds)
        {
            if (trgt == target)
            {
                needToCgange = false;
                break;
            }
        }
        if (needToCgange)
        {
            GetRandomTargetFromBounds();
        }
    }

    public virtual void Glance()
    {
        if (Info.CharmLevel < enemyLevel) return;

        GameObject sighPoint = gameObject.transform.Find("View Point ").gameObject;
        int x = gameObject.transform.localScale.x > 0 ? -1 : 1;
        for (float y = -1; y < 1; y += 0.1f)
        {
            var hit = Physics2D.Raycast(sighPoint.transform.position, new Vector2(x, y), 10f, layermask);
            Debug.DrawRay(sighPoint.transform.position, new Vector2(x * 10, y * 10), Color.blue);
            if (!hit)
            {
                continue;
            }
            

            if (hit.collider.CompareTag("Player") && currentState != (int)States.chasing)
            {
                StartChasing();
            }
            
        }
    }
    #endregion

    #region Corutines
    IEnumerator StayWatch(float period)
    {
        speed = 0f;
        float timeToWait = r.Next(7);
        while (timeToWait > 0)
        {
            if (currentState == (int)States.chasing)
                break;
            yield return new WaitForSeconds(period);
            timeToWait -= period;
        }
        speed = 1.2f;
        GetRandomTargetFromBounds();
    }
    #endregion
}
