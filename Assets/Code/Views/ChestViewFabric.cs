using Assets.Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Views
{
    class ChestViewFabric : GameObjectsLoader, IGameObjectFabric
    {
        GameObject _chestPrefab;
        public GameObject CreateGameObject()
        {
            return CreateObjectFromFile(
                ref _chestPrefab, "Prefabs/Loot/TreasureСhest");
        }

    }
}
