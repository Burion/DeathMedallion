using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingWeb : MonoBehaviour
{
    [SerializeField] GameObject groundWeb;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ground"))
        {
            Instantiate(groundWeb, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if (col.CompareTag("Player"))
        {
            col.SendMessage("GotHit");
            Destroy(gameObject);
        }
    }
}
