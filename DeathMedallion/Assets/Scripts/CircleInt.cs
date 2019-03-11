using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleInt
{
    public int MaxValue { get; set; }
    int currentValue;
    public int CurrentValue
    {
        get
        {
            return currentValue;
        }
        set
        {
            if(value >= MaxValue)
            {
                if (currentValue != MaxValue - 1)
                    currentValue = MaxValue - 1;
                else
                    currentValue = 0;
            }
            else if(value < 0)
            {
                currentValue = MaxValue - 1;
            }
            else
            {
                currentValue = value;
            }
        }
    }

    public CircleInt(int currentValue, int MaxValue)
    {
        this.currentValue = currentValue;
        this.MaxValue = MaxValue; 
    }

    public static CircleInt operator +(CircleInt cInt, int Int)
    {
        cInt.CurrentValue += Int;
        return new CircleInt(cInt.CurrentValue, cInt.MaxValue);
    }

    public static CircleInt operator -(CircleInt cInt, int Int)
    {
        cInt.CurrentValue -= Int;
        return new CircleInt(cInt.CurrentValue, cInt.MaxValue);
    }

    public static CircleInt operator --(CircleInt cInt)
    {
        cInt.CurrentValue--;
        return new CircleInt(cInt.CurrentValue, cInt.MaxValue);
    }
    public static CircleInt operator ++(CircleInt cInt)
    {
        cInt.CurrentValue++;
        return new CircleInt(cInt.CurrentValue, cInt.MaxValue);
    }
    public static implicit operator int(CircleInt cInt)
    {
        return cInt.CurrentValue;
    }
}
