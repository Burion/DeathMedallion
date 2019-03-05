using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    GameObject Player;
    bool attached;
    public float sighRange;
    Vector3 target;
    float speed = 10f;
    bool reached;
    void Start()
    {
        reached = true;
        attached = true;
        Player = GameObject.Find("Hero");
    }


    void Update()
    {
        if (attached)
        {
            target = new Vector3(Player.transform.position.x, Player.transform.position.y, transform.position.z);
            if (Vector2.Distance(Player.transform.position, transform.position) > 0.1f && !reached)
            {
                Slide();
            }
            else
            {
                transform.position = target;
                reached = true;
            }
        }

        else
        {
            Slide();
        }

        if (Input.GetAxisRaw("SighVertical") != 0 || Input.GetAxisRaw("SighHorizontal") != 0)
        {
            Debug.Log(Input.GetAxisRaw("SighVertical"));
            Debug.Log(Input.GetAxisRaw("SighHorizontal"));
            attached = false;
            LookAround();
            reached = false;
        }
        else
            attached = true;
 
        
    }
    void LookAround()
    {
        if(Input.GetAxisRaw("SighVertical") == -1)
        {
            target = new Vector3(Player.transform.position.x, Player.transform.position.y + sighRange, transform.position.z);
        }
        if (Input.GetAxisRaw("SighVertical") == 1)
        {
            target = new Vector3(Player.transform.position.x, Player.transform.position.y - sighRange, transform.position.z);
        }
        if (Input.GetAxisRaw("SighHorizontal") == 1)
        {
            target = new Vector3(Player.transform.position.x + sighRange, Player.transform.position.y, transform.position.z);
        }
        if (Input.GetAxisRaw("SighHorizontal") == -1)
        {
            target = new Vector3(Player.transform.position.x - sighRange, Player.transform.position.y, transform.position.z);
        }

    }
    void Slide()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, target, step);
        
    }
}
