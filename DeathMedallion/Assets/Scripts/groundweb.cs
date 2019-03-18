using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundweb : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<Unit>().speed =  col.GetComponent<Unit>().spdmng.ChangeSpeed(-3f);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<Unit>().speed = col.GetComponent<Unit>().spdmng.RevertSpeed();
        }
    }
}
