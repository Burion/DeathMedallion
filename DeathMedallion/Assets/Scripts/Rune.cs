using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : Interactable
{
    //!!! TODO make this class more OOP

    public GameObject CanvasAlert;
    Color32 activatedColor;
    public override void OnEnable()
    {
        base.OnEnable();
        IsOneTimeInteraction = true;
        activatedColor = new Color32(255, 16, 0, 255);
        Interacted += new OnInteract(mng.LevelUpBots);
        Interacted += new OnInteract(RedRune);
        Interacted += new OnInteract(ShowAlert);
    }
    private void Start()
    {
        transform.Find("Message").gameObject.SetActive(false);
    }
    

    void RedRune()
    {
        GetComponentInChildren<SpriteRenderer>().color = activatedColor;
        transform.Find("Message").gameObject.SetActive(false);
    }
    void ShowAlert()
    {
        CanvasAlert.SetActive(true);
    }
}
