using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class NotePrefab : MonoBehaviour
{ 
    public string prev;
    public string text;

    public void InitNote(string prev, string text)
    {
        this.prev = prev;
        this.text = text;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = prev;

    }
}
