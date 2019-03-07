using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item<T> : MonoBehaviour
{
    //properties
    public int coins;

    public delegate void TakenDelegate(); //set of methods that are calld when item is taken
    public event TakenDelegate OnTaken; //enent that occures when item is taken

    private void OnEnable()
    {
        
    }
    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            OnTaken();
            Destroy(gameObject);
        }
    }

    #region Methods when taken
    public void AddCoins()
    {
        Info.Coins += coins;
    }
    public void AddItem(int itemHash)
    {

    }
    public void AddNote(T note)
    {

    }
    #endregion
}
