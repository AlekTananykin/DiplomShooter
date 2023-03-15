using Assets.Code.Interfaces;
using Assets.Code.Things;
using Assets.Code.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Controllers
{
    internal sealed class ShootoutController<BombFabricTemplate> : 
        IAction, IInitialization
        where BombFabricTemplate: IGameObjectFabric, new()
    {
        GameObjectsPool<BombFabricTemplate> _bombsPool;
        const int _poolSize = 23;
        const int _hitCount = 22;

        public void Initialize()
        {
            _bombsPool =
                new GameObjectsPool<BombFabricTemplate>(
                    new BombFabricTemplate(), _poolSize);
        }

        public bool Attack(Vector3 place, Vector3 targetPoint, Thing actionThing)
        {
            GameObject bullet = _bombsPool.Create();
            bullet.transform.position = place;
            bullet.SetActive(true);
            Rigidbody rigidBody = bullet.GetComponent<Rigidbody>();
            rigidBody.velocity = (targetPoint - place).normalized * 50f;

            BombScript bombScript = bullet.GetComponent<BombScript>();
            bombScript.OnBombCall += BulletFlyCompleeted;
            return true;
        }

        internal void BulletFlyCompleeted(GameObject bullet, Collider target)
        {
            BombScript bombScript = bullet.GetComponent<BombScript>();
            bombScript.OnBombCall -= BulletFlyCompleeted;
            _bombsPool.Intake(ref bullet);

            if (target.TryGetComponent(out IReactToHit react))
                react.Hit((uint)_hitCount);
        }
    }
}
