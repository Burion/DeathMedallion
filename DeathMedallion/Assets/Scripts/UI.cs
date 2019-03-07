using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    public TextMeshProUGUI coins;


    private void Update()
    {
        //TODO *FUTURE* beautiful appear
        coins.text = Info.Coins.ToString();
    }

}
