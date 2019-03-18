using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    GameObject message;
    public delegate void OnInteract();
    public event OnInteract Interacted;
    public Manager mng;

    public bool IsOneTimeInteraction;
    bool interactred = false;

    public virtual void OnEnable()
    {
        mng = GameObject.Find("Manager").GetComponent<Manager>();
        transform.Find("Message").gameObject.SetActive(false);
        Interacted += new OnInteract(InteractedTrue);
    }

    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (IsOneTimeInteraction)
            if (interactred) return;
        if (!col.CompareTag("Player")) return;
        transform.Find("Message").gameObject.SetActive(true);
    }
    public virtual void OnTriggerExit2D(Collider2D col)
    {
        if (IsOneTimeInteraction)
            if (interactred) return;
        if (!col.CompareTag("Player")) return;
        transform.Find("Message").gameObject.SetActive(false);
    }
    public virtual void OnTriggerStay2D(Collider2D col)
    {
        if (IsOneTimeInteraction)
            if (interactred) return;
        if (!col.CompareTag("Player")) return;
        if (Input.GetButtonDown("Interact"))
        {
            Interacted();
        }
    }
    void InteractedTrue()
    {
        interactred = true;
        transform.Find("Message").gameObject.SetActive(false);
    }
}
