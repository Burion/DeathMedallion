using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    Unit thisUnit;
    private void Start()
    {
        thisUnit = gameObject.GetComponentInParent<Unit>();
    }
    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.CompareTag("Ground"))
        {
           thisUnit.grounded = true;
        }
    }
    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Ground"))
        {
            thisUnit.grounded = false;
        }
    }
}
