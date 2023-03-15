using Assets.Code.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonScript : MonoBehaviour, IReactToHit
{
    public void Hit(uint damage)
    {
        Damage?.Invoke(damage);
    }

    internal Action<uint> Damage;
}
