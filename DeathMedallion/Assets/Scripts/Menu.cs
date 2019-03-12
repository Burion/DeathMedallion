using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IMenu
{
    void CheckForPressing();
}

public class Menu : MonoBehaviour
{
    public bool pressed = false;
    public MenuElement Notes;
    public MenuElement Pause;
    public MenuElement currentMenu;

    public CircleInt choice;

    public virtual void OpenMenu(MenuElement menu)
    { 
        MenuElement temp = currentMenu;
        currentMenu = menu;
        currentMenu.GObj.SetActive(true);
        temp.GObj.SetActive(false);

    }
    public virtual void OnChoiceChange()
    {
        foreach (Image child in currentMenu.Grid.GetComponentsInChildren<Image>())
        {
            child.color = new Color32(255, 255, 255, 255);
        }
        currentMenu.Grid.transform.GetChild(choice).GetComponent<Image>().color = new Color32(255, 30, 0, 255);
        pressed = true;
    }
    public void FixPressing()
    {
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("UpDown") == 0)
        {
            pressed = false;
        }
    }
}
