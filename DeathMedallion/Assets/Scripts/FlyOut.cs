using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyOut : MonoBehaviour
{
    System.Random random = new System.Random();

    private void OnEnable()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(random.Next(-100, 100) * .01f, 1)*100);
        StartCoroutine(Fading());
    }

    IEnumerator Fading()
    {
        byte alpha = 255;
        while (alpha > 0)
        {
            GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, alpha);
            yield return new WaitForEndOfFrame();
            alpha-=10;
            if(alpha < 15)
            {
                break;
            }
        }
        Destroy(gameObject);
    }
}
