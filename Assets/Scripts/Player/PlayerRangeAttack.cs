using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRangeAttack : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Rigidbody>().velocity = Vector3.up;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(this.gameObject.tag) && !other.isTrigger)
        {
            other.TryGetComponent<IDamageable>(out IDamageable damage);
            damage?.TakeDamage(1);
            Destroy(this.gameObject);
        }
    }
}
