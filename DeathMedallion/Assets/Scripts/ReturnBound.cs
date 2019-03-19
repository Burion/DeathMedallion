using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnBound : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        collision.gameObject.transform.position = transform.parent.Find("Return Point").transform.position;
        collision.SendMessage("GotHit");
    }
}
