using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    Player player;
    public GameObject hitPrefab;
    private void Start()
    {
        player = GetComponentInParent<Player>();

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.tag)
        {
            case "Enemy":
                Instantiate(hitPrefab, gameObject.transform.position, Quaternion.identity);
                Enemy aim = col.GetComponent<Enemy>();
                aim.GotHit(transform.lossyScale.x > 0 ? 1 : -1);
                Debug.Log("Hit");
                break;

            case "Ambient":
                if(col.GetComponent<AmbientThing>() is MovableAmbientThing)
                {
                    col.GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.lossyScale.x, 1).normalized * 300);
                }
                col.GetComponent<AmbientThing>().GetHit();
                
                break;
        }
    }

}
