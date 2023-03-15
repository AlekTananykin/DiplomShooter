using Assets.Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Auxiliary
{
    internal class PersonMover: IPersonMover
    {
        private ILeash _leash;
        private float _rotationSpeed;
        private Vector3 _currentPoint;
        internal PersonMover(ILeash leash, float rotationSpeed)
        {
            _leash = leash;
            _currentPoint = _leash.GetLastPoint();
        }

        public bool IsNeedMove => _leash.IsNeedMove;

        public void AddPoint(Vector3 point)
        {
            _leash.AddPoint(point);
        }

        public (Vector3 nextPoint, Vector3 direction) 
            Execute(float deltaTime, float speed)
        {
            Vector3 nextPoint = _leash.Execute(deltaTime, speed);
            Vector3 direction = nextPoint - _currentPoint;
            _currentPoint = nextPoint;

            return (nextPoint, direction);
        }
    }
}
