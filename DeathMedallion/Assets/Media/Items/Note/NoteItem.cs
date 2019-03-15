using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteItem : Item<int>
{
    [SerializeField] GameObject indicator;

    private void OnEnable()
    {
        OnTaken += new TakenDelegate(AddNote);
        OnTaken += new TakenDelegate(ShowIndicator);        
    }
    void ShowIndicator()
    {
        indicator.SetActive(true);
    }
}
