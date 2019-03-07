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
    }

    public override void Chasing()
    {
        if (!PlayerIsReachable())
        {
            speed = spdmng.RevertSpeed();
            MovingTowardsTarget(mng.Player.transform.position);
        }
        else
        {
            StartCombatMode();
        }
    }
    public override void StartChasing()
    {
        CurrentState = (int)States.chasing;
    }
    public override void StartCombatMode()
    {
        CurrentState = (int)States.combat;
        StartCoroutine(Attack());
    }
    #region Functions
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
    #endregion
    #region Corutines
    IEnumerator Attack()
    {
        GameObject Ball = Instantiate(ball, transform.Find("throwPoint").transform.position, Quaternion.identity); //FIXME create spawning using vector2.identity
        Vector2 dir = transform.Find("throwPoint").transform.position - mng.Player.transform.position;

        Ball.GetComponent<Rigidbody2D>().AddForce(-dir.normalized*500);
        yield return new WaitForSeconds(3);
        if (PlayerIsReachable())
        {
            StartCoroutine(Attack());
        }
        else
        {
            StartChasing();
        }
    }
    
    #endregion
}
