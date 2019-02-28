using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientThing : MonoBehaviour
{
    public int health;

    public void GetHit()
    {
        GetComponent<Animator>().Play("hit");

        if (--health <= 0)
        {
            //Destroy Animation
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        health = 3;
    }
}
