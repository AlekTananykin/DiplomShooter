using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal delegate void OnZondTriggerEnter(GameObject target);
internal delegate void OnZondTriggerExit(GameObject target);

public class ZondScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        On_Zond_Trigger_Enter?.Invoke(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        On_Zond_Trigger_Exit?.Invoke(other.gameObject);
    }

    internal OnZondTriggerEnter On_Zond_Trigger_Enter;
    internal OnZondTriggerExit On_Zond_Trigger_Exit;
}
