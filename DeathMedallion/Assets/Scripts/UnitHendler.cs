using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitHendler
{
    public static int GetHealth(Unit unit)
    {
        return unit.Health;
    }

    public static void SetHealth(Unit unit, int x)
    {
        unit.Health = x;
    }

    public static void SetLevel(Unit enemy, int level)
    {
        enemy.Health += level * 10;
    }
}
