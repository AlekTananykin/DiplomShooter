using Assets.Code.Auxiliary;
using Assets.Code.Interfaces;
using Assets.Code.Things;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Controllers
{
    internal class PrickAttackController : IAction
    {
        private float _attackDistance = 1f;
        private const uint _hitCount = 100;

        public bool Attack(Vector3 place, Vector3 targetPoint, Thing actionThing)
        {
            Ray ray = new Ray(place, (targetPoint - place).normalized);
            if (!Physics.Raycast(ray, out RaycastHit hitInfo, _attackDistance))
                return true;

            if (hitInfo.collider.gameObject.TryGetComponent(
                out IReactToHit react))
                react.Hit(_hitCount);

            return true;
        }
    }
}
