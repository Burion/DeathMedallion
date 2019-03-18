using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Portal : Interactable
{

    //Portal logic (future)


    public override void OnEnable()
    {
        base.OnEnable();
        IsOneTimeInteraction = true; // Only for Demo level

    }

    void PortalPlayer()
    {
        Debug.Log("Portalling player to another level...");
    }
}
