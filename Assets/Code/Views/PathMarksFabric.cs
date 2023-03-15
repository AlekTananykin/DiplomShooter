using Assets.Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Views
{
    class PathMarksFabric: GameObjectsLoader, IGameObjectFabric
    {
        private GameObject _pathMarkPrefab;
        public GameObject CreateGameObject()
        {
            return CreateObjectFromFile(
                ref _pathMarkPrefab, "Prefabs/Heroes/PathMark");
        }
    }
}
