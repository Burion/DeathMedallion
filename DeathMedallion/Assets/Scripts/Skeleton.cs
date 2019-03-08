using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2 ot
public class Skeleton : Enemy
{
    //1 ot
    public delegate void ReachPlayer();

    event ReachPlayer ReachingPlayer;
    Animation additonAnim;
    //Death Objects
    [SerializeField] List<GameObject> bodyParts;
    [SerializeField] List<GameObject> deadBodyParts;
    
    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        base.Start();
        speed = 1.2f;
        target = new Vector2(mng.ChasePlayer(gameObject).x, mng.ChasePlayer(gameObject).y);
        SetBounds(6);
        FoundPlayer += new AlarmCons(ShowAlarm);
        ReachingPlayer += new ReachPlayer(StartCombatMode);
        UnitHendler.SetHealth(this, 4);
        hitable = false;
    }
    public override void UpdateAnimator()
    {
        base.UpdateAnimator();
        anim.SetBool("grounded", isGrounded);
        anim.SetFloat("speed", speed);
        anim.SetFloat("velocityY", rb.velocity.y);
    }

    /// <summary>
    /// chasing the player accoor
    /// </summary>
    public override void Chasing() //tripple nested
    {
        if (isGrounded)
        {
            float y1 = mng.GetNearest(transform.GetChild(0).position).y;
            target = mng.ChasePlayer(gameObject.transform.GetChild(0).gameObject);
            float y2 = target.y;
            if (y2 - y1 > 0 && ableToJump)
            {
                if (y2 - y1 < 3)//magic number
                {
                    Debug.Log(y2);
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    Jump(270);//height -||-
                    StartCoroutine("BlockJump");
                }

                if (Mathf.Abs(y2 - y1) >= 3)// -||-
                {
                    currentState = (int)States.patrolling;
                }
            }

        }
        //transform.position = Vector2.MoveTowards(transform.position, target, 0.1f);
        MovingTowardsTarget();

        if (IsNearThePlayer())
        {
            ReachingPlayer(); //событие, которое переводит бота в режим сражение
        }


    }

    public override void Wandering()
    {
        if (!IsInResponsiveZone())
        {
            if (isGrounded)
            {
                Vector2 oldPos = GetNearest();
                target = mng.ChasePoint(gameObject.transform.GetChild(0).gameObject, Bounds[Bounds.Count / 2]);

                if (Mathf.Abs(target.y - oldPos.y) > 0 && ableToJump)
                {
                    if (IsAbleToReach(target.y, oldPos.y))
                    {
                        Debug.Log(target.y);
                        rb.velocity = new Vector2(rb.velocity.x, 0);
                        Jump(270);
                        StartCoroutine("BlockJump");
                    }
                }
                if (Mathf.Abs(target.y - oldPos.y) >= 3)
                {
                    Bounds.Clear();
                    Bounds = mng.GetPlaneVertices(mng.GetNearest(transform.GetChild(0).position), 6);
                    GetRandomTargetFromBounds();
                }
            }

            MovingTowardsTarget();
        }
        else
        {
            FixWandering();
            if (IsOnThePoint())
            {
                if (speed > 0)
                    StartCoroutine("StayWatch", 0.1f);
            }
            MovingTowardsTarget();
        }
        Glance();

        
    }

    bool IsAbleToReach(float y1, float y2)
    {
        return (y1 - y2 > 0 && y1 - y2 < 3 && ableToJump);
    }
    
    void StopMoving()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        speed = 0f;
    }

    void ShowAlarm()
    {
        gameObject.transform.GetChild(2).gameObject.SetActive(true);
    }

    void StopChasing()
    {
        currentState = (int)States.patrolling;
    }

    public override void StartCombatMode()
    {
        base.StartCombatMode();
        StartCoroutine("WaitToSetSpeedZero");
        StartCoroutine("Dash");
    }

    void CheckCombatState()
    {
        if (currentState == (int)States.death)
        {
            return;
        }
        if (IsNearThePlayer())
        {
            if (CanHitPlayer())
            {
                StartCoroutine(Attack());
            }
            else
            {
                StartCoroutine(Dash());
            }
        }
        else
        {
            StartChasing();
        }
    }

    public override void GotHit(int dir)
    {
        if (!hitable)
        {
            anim.Play("pair");
            //anim.SetTrigger("pair");
            //StartCoroutine(FreezeTime(0.1f));
            //GetRecoil(dir);
            StopCoroutine(DefenceMode());
            StartCoroutine(DefenceMode());
        }
        else
        {
            hitable = false;
            anim.Play("gothit");
            GetRecoil(dir);
            Health--;
        }
    }

    bool IsNearThePlayer()
    {
        List<Vector2> testplane = mng.GetPlaneVertices(gameObject.transform.GetChild(0).position, 6);
        Vector2 playerPos = mng.GetNearest(mng.Player.transform.position);
        foreach (Vector2 pos in testplane)
        {
            if (playerPos == pos)
            {
                Debug.Log("Near");
                return true;
            }
        }
        return false;
    }

    bool CanHitPlayer()
    {
        return Vector2.Distance(transform.position, mng.Player.transform.position) < 2;
    }

    #region Animations Methods
    public void SetHittable(bool value)
    {
        hitable = value;
    }

    public void HitableTrue()
    {
        SetHittable(true);
    }

    public void HitableFalse()
    {
        SetHittable(false);
    }
    #endregion


    #region Corutiens
    IEnumerator BlockJump()
    {
        ableToJump = false;
        yield return new WaitForSeconds(0.1f);
        ableToJump = true;
    }

    IEnumerator DefenceMode()
    {
        yield return new WaitForSeconds(r.Next(2, 8) * 0.5f);
        CheckCombatState();
    }

    IEnumerator WaitToSetSpeedZero()
    {
        yield return new WaitUntil(() => isGrounded);
        rb.velocity = new Vector2(0, 0);
    }

    IEnumerator Dash()
    {
        speed = 3;
        MovingTowardsTarget(mng.Player.transform.position);
        yield return new WaitForSeconds(0.2f);
        StopMoving();
        StartCoroutine(DefenceMode());
    }

    IEnumerator Attack()
    {
        anim.SetBool("attacking", true);
        yield return new WaitForSeconds(1);
        Debug.Log("Hit!");
        anim.SetBool("attacking", false);
        yield return new WaitForSeconds(1);
        CheckCombatState();
    }

    IEnumerator FreezeTime(float time)
    {
        Time.timeScale = 0f;
        yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(time));
        Time.timeScale = 1;
    }
    #endregion
}

public static class CoroutineUtil
{
    public static IEnumerator WaitForRealSeconds(float time)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
    }
}
