using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : MonoBehaviour
{
    public delegate void OnInteract();
    public event OnInteract Interacted;
    Manager mng;
    public GameObject CanvasAlert;
    private void OnEnable()
    {
        mng = GameObject.Find("Manager").GetComponent<Manager>();
        Interacted += new OnInteract(mng.LevelUpBots);
        Interacted += new OnInteract(RedRune);
        Interacted += new OnInteract(ShowAlert);
    }
    private void Start()
    {
        transform.Find("Alert").gameObject.SetActive(false);
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            transform.Find("Alert").gameObject.SetActive(true);
        }
    }

    void RedRune()
    {
        GetComponentInChildren<SpriteRenderer>().color = new Color32(255, 16, 0, 255);
    }
    void ShowAlert()
    {
        CanvasAlert.SetActive(true);
    }
    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            transform.Find("Alert").gameObject.SetActive(false);
        }
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (Input.GetButtonDown("Interact"))
            {
                Interacted();
            }
        }
    }
}
