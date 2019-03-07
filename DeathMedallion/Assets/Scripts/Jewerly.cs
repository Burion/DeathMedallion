using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jewerly : Item<int>
{

    private void OnEnable()
    {
        OnTaken += new TakenDelegate(AddCoins);
    }

}
