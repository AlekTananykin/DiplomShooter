using Assets.Code.Auxiliary;
using Assets.Code.Interfaces;
using Assets.Code.Things;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate IList<Thing> GetStorage();

public class BagScript : MonoBehaviour, IStorage
{
    public IList<Thing> GetItems()
    {
        if (null == Get_Storage)
            return null;

        return Get_Storage();
    }

    public GetStorage Get_Storage;
}
