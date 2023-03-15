using Assets.Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Views
{
    class BombViewFabric : GameObjectsLoader, IGameObjectFabric
    {
        GameObject _bombPrefab;
        public GameObject CreateGameObject()
        {
            return CreateObjectFromFile(
                ref _bombPrefab, "Prefabs/Weapon/Bomb");
        }
    }
}
