using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject currentMenu;

    public CircleInt choice = new CircleInt(0, 3);

    public virtual void OpenMenu(GameObject menu)
    { 
        GameObject temp = currentMenu;
        currentMenu = menu;
        currentMenu.SetActive(true);
        temp.SetActive(false);

    }
}
