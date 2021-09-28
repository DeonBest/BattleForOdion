using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerParent : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (GetComponentInParent<PlayerController>() != null)
            GetComponentInParent<PlayerController>().OnTriggerEnter2D(other);
    }
}
