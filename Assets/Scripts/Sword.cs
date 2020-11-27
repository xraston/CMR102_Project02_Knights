using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public int damage; // the amount of damage the script gets from KnightManager
    public float damageInterval; // the amount of time to wait between damage instances
    private float timeTillNextDamage; // the amount of time needed to pass till the next damage instance is applied

    void OnTriggerEnter(Collider collider)
    {
        if(Time.time >= timeTillNextDamage) // prevents the collider from applying multiple damage instances in one sword swing
        {
            if (collider.transform.GetComponent<KnightManager>())
            {
                    collider.transform.GetComponent<KnightManager>().ApplyDamage(damage);
                    Debug.Log("HIT! " + damage);
            }
            timeTillNextDamage = Time.time + damageInterval; // updates the time difference
        }

    }
}
