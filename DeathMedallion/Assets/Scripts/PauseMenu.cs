using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : Menu
{
    [SerializeField] GameObject Notes;
    [SerializeField] GameObject Pause;

    private void OnEnable()
    {
        currentMenu = Pause;
        choice.CurrentValue = 0;
        OnChoiceChange();
    }


    void OnChoiceChange()
    {
        foreach (Image child in transform.Find("Menu").GetComponentsInChildren<Image>())
        {
            child.color = new Color32(255, 255, 255, 255);
        }
        transform.Find("Menu").GetChild(choice).GetComponent<Image>().color = new Color32(255, 30, 0, 255);
    }

    private void Update()
    {
        CheckForPress();
    }
    void CheckForPress()
    {
        if (Input.GetButtonDown("Submit"))
        {
            switch (choice)
            {
                case 0:
                    
                    break;
                case 1:
                    OpenMenu(transform.Find("Notes").gameObject);
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            choice++;
            OnChoiceChange();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            choice--;
            OnChoiceChange();
        }

    }
    }
