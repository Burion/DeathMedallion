using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : Menu, IMenu
{


    private void OnEnable()
    {
        choice = new CircleInt(0, Pause.GObj.transform.childCount);
        currentMenu = Pause;
        choice.CurrentValue = 0;
        OnChoiceChange();
        if(GameObject.Find("note_indicator") != null)
        {
            OpenMenu(Notes);
            GameObject.Find("note_indicator").SetActive(false);
        }
    }


    private void Update()
    {
        CheckForPressing();
    }
    public void CheckForPressing()
    {
        if (Input.GetButtonDown("Submit"))
        {
            switch (choice)
            {
                case 0:
                    transform.parent.gameObject.SetActive(false);
                    Service.SetGame(true);
                    break;
                case 1:
                    OpenMenu(Notes);
                    break;
            }
        }
        if (!pressed)
        {
            if (Input.GetAxisRaw("UpDown") == -1)
            {
                choice++;
                OnChoiceChange();
            }

            if (Input.GetAxisRaw("UpDown") == 1)
            {
                choice--;
                OnChoiceChange();
            }
        }
        FixPressing();
    }
    }
