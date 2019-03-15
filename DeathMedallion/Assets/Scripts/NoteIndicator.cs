using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NoteIndicator : MonoBehaviour
{

    [SerializeField] int repeats;

    private void OnEnable()
    {
        repeats = 5;
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        int alpha = 255;
        while (alpha > 155)
        {
            GetComponentInChildren<Image>().color = new Color32(255, 255, 255, (byte)alpha);
            yield return new WaitForEndOfFrame();
            alpha -= 4;
        }
        while (alpha < 254)
        {
            GetComponentInChildren<Image>().color = new Color32(255, 255, 255, (byte)alpha);
            yield return new WaitForEndOfFrame();
            alpha += 4;
        }
        if (--repeats > 0)
            StartCoroutine("Blink");
        else
            gameObject.SetActive(false);
    }
}
