using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventController : MonoBehaviour
{
    [SerializeField] GameObject weaponTrigger;
    [SerializeField] Transform head;
    [SerializeField] GameObject deadHead;
    Skeleton parent;

    private void Start()
    {
        parent = GetComponentInParent<Skeleton>();
    }
    public void ShowWeaponTrigger()
    {
        weaponTrigger.SetActive(true);
    }

    public void HideWeaponTrigger()
    {
        weaponTrigger.SetActive(false);
    }
    #region animation functions

    public void ThrowHead()
    {
        Instantiate(deadHead, head.position, Quaternion.identity);
    }
    public void BecomeHittable()
    {
        parent.HitableTrue();
    }
    public void BecomeUnHittable()
    {
        Debug.Log("UnHittable");
        parent.HitableFalse();
    }
    #endregion
}
