using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoSign : MonoBehaviour
{
    private void OnEnable()
    {
        transform.Find("Canvas").gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            transform.Find("Canvas").gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            transform.Find("Canvas").gameObject.SetActive(false);
        }
    }
}
