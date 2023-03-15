using Assets.Code.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Views
{
    class GameObjectsLoader
    {
        public GameObject CreateObjectFromFile(
            ref GameObject prefab, string prefabPath)
        {
            if (null == prefab)
            {
                prefab = (GameObject)Resources.Load(prefabPath);
                if (null == prefab)
                    throw new GameException("CreateObjectFromFile: " +
                        "Prefab can't be loaded from file \"" + prefabPath + "\"");
            }
             var pathMark = GameObject.Instantiate(prefab);
            return pathMark;
        }
    }
}
