using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Enemy
{


    [SerializeField] GameObject ball;
    public override void __init__(float speed, int dmg)
    {
        base.__init__(speed, dmg);
        SetBounds(6);
        Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), mng.Player.GetComponent<Collider2D>());
    }
    private void Start()
    {
        __init__(1.2f, 2);
        enemyLevel = 0;
        Health = 4;
    }
    public override void Chasing()
    {

    }

    public override void StartChasing()
    {
        currentState = (int)States.chasing;
    }
    public override void StartCombatMode()
    {
        currentState = (int)States.combat;
        StartCoroutine(Attack());
    }
    #region Functions
    public override void Glance()
    {
        GameObject sighPoint = transform.Find("View Point ").gameObject;
        float x = 1;
        float y;
        for (; x > -1; x -= 0.01f)
        {
            y = 1 - Mathf.Abs(x);
            var hit = Physics2D.Raycast(sighPoint.transform.position, new Vector2(x, y), 10f, layermask);
            Debug.DrawRay(sighPoint.transform.position, new Vector2(x, y) * 10, Color.blue);

            if (hit)
                if (hit.collider.CompareTag("Player") && currentState != (int)States.chasing)
                {
                    StartCombatMode();
                    break;
                }
        }
    }

    public bool PlayerIsVisible()
    {
        GameObject sighPoint = transform.Find("View Point ").gameObject;
        float x = 1;
        float y;
        for (; x > -1; x -= 0.01f)
        {
            y = 1 - Mathf.Abs(x);
            var hit = Physics2D.Raycast(sighPoint.transform.position, new Vector2(x, y), 10f, layermask);
            Debug.DrawRay(sighPoint.transform.position, new Vector2(x, y) * 10, Color.blue);

            if (hit)
                if (hit.collider.CompareTag("Player") && currentState != (int)States.chasing)
                {
                    return true;
                }
        }
        return false;
    }
    
    bool PlayerIsReachable()
    {
        List<Vector2> Plane = mng.GetPlaneVertices(gameObject.transform.Find("groundChecker").position, 4);
        foreach (Vector2 point in Plane)
        {
            if (mng.Player.GetComponent<Unit>().GetNearest() == point)
            {
                return true;
            }
        }
        return false;
    }

    bool CanHitPlayer()
    {

        var hit = Physics2D.Raycast(transform.Find("View Point ").transform.position, new Vector2(1, 1), 10f, layermask);
        return false;
    }

    public override void UpdateAnimator()
    {
        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
    }
    #endregion
    #region Corutines
    IEnumerator Attack()
    {
        GameObject Ball = Instantiate(ball, transform.Find("throwPoint").transform.position, Quaternion.identity); //FIXME create spawning using vector2.identity
        Vector2 dir = transform.Find("throwPoint").transform.position - mng.Player.transform.position;

        Ball.GetComponent<Rigidbody2D>().AddForce(-dir.normalized*500);
        yield return new WaitForSeconds(3);

        if (PlayerIsVisible())
        {
            StartCoroutine(Attack());
        }
        else
        {
            currentState = (int)States.patrolling;
            if (!IsInResponsiveZone())
            {
                SetBounds(6);
            }
            speed = spdmng.RevertSpeed();
        }
    }
    
    #endregion
}
