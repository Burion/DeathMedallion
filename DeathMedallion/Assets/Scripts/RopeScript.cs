using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour
{
    [SerializeField] GameObject mount_1;
    [SerializeField] GameObject mount_2;
    [SerializeField] GameObject rope_tile;
    Vector2 dir;
    private void Start()
    {
        rope_tile.transform.position = new Vector3(0, 0, 0);

        transform.position = mount_1.transform.position;
        while (Vector2.Distance(transform.position, mount_1.transform.position) < Vector2.Distance(mount_1.transform.position, mount_2.transform.position))
        {
            Step();
        }
    }

    private void Update()
    {

    }

    void Step()
    {
        dir = (mount_2.transform.position - mount_1.transform.position).normalized * rope_tile.transform.localScale.x * 0.075f;
        GameObject placed = Instantiate(rope_tile, transform.position, Quaternion.identity, transform.parent);
        placed.transform.rotation = Quaternion.LookRotation(dir);
        transform.position = new Vector2(transform.position.x + dir.x, transform.position.y + dir.y);
    }
}
