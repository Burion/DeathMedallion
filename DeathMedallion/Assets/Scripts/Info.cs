using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Info
{
    
    public static int CharmLevel;
    public static bool IsGameOn = true;
    private static int coins;

    public static int Coins
    {
        get
        {
            return coins;
        }
        set
        {
            coins = value;
        }
    }
}
