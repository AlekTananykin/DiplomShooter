using System.Collections;
using System.Collections.Generic;
using UnityEngine;


internal delegate void OnPathMarkTrigger(GameObject collider);

public class PathMarkScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        OnPathMatkCall?.Invoke(transform.gameObject);
    }

    internal OnPathMarkTrigger OnPathMatkCall;
}
