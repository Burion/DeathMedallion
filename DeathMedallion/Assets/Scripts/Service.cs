using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum States
{
    sleep = 0,
    patrolling = 1,
    chasing = 2,
    combat = 3,
    death = 4
}

public static class Service
{

    public static void SetGame(bool choice)
    {
        Time.timeScale = choice? 1f : 0f;
        Info.IsGameOn = choice;
    }

}
