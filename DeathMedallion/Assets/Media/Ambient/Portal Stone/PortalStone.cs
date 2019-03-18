using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalStone : Interactable, ILevelable
{
    public override void OnEnable()
    {
        base.OnEnable();
        IsOneTimeInteraction = true;
        Level = 1;
        Interacted += new OnInteract(ShowPortal);
        GetComponent<Collider2D>().enabled = false;
    }
    public int Level { get; set; }

    public void SetVisability(bool choice)
    {
        transform.Find("rune").GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255);
        GetComponent<Collider2D>().enabled = true;
    }

    void ShowPortal()
    {
        transform.GetChild(1).gameObject.SetActive(true);
    }
}
