using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal delegate void OnBombTrigger(GameObject bullet, Collider target);
internal sealed class BombScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider target)
    {
        OnBombCall?.Invoke(this.gameObject, target);
    }

    internal OnBombTrigger OnBombCall;
}
