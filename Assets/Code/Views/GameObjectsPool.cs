using Assets.Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Views
{
    class GameObjectsPool<FabricTemplate> 
        where FabricTemplate : IGameObjectFabric
    {
        private FabricTemplate _fabric;
        private Queue<GameObject> _storage;
        private readonly uint _poolSize;
        public GameObjectsPool(FabricTemplate fabric, uint poolSize)
        {
            _fabric = fabric;
            _storage = new Queue<GameObject>();
            _poolSize = poolSize;
        }

        public GameObject Create()
        {
            if (0 < _storage.Count)
            {
                GameObject go = _storage.Dequeue();
                go.SetActive(true);
                return go;
            }
            
            return _fabric.CreateGameObject();
        }

        public void Intake(ref GameObject go)
        {
            if (_poolSize > _storage.Count)
            {
                _storage.Enqueue(go);
                go.SetActive(false);
            }
            else
                GameObject.Destroy(go);
            go = null;
        }
    }
}
