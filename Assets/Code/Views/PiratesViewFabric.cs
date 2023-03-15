using Assets.Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Views
{
    class PiratesViewFabric : GameObjectsLoader, IGameObjectFabric
    {
        GameObject _piratePrefab;
        public GameObject CreateGameObject()
        {
            return CreateObjectFromFile(
                ref _piratePrefab, "Prefabs/Enemies/Pirate");
        }
    }
}
