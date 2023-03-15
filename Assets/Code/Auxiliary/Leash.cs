using Assets.Code.Interfaces;
using Assets.Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Auxiliary
{
    internal abstract class Leash<Track>: ILeash
        where Track: ITrack, new()
    {
        private float _treveledDistance = 0;
        private float _targetDistance = 0;

        protected Vector3 _lastPoint;
        protected ITrack _track = new Track();

        public Leash(Vector3 currentPoint)
        {
            _lastPoint = new Vector3(currentPoint.x, 
                currentPoint.y, currentPoint.z);
            _targetDistance = 0;
            _treveledDistance = 0;
        }

        public Vector3 Execute(float deltaTime, float speed)
        {
            if (_track.IsEmpty)
                return _lastPoint;

            Vector3 arrivalPoint = _track.GetPoint();

            if (0 == _targetDistance)
                _targetDistance = (_lastPoint - arrivalPoint).magnitude;

            _treveledDistance = Mathf.Clamp(
                _treveledDistance + speed * deltaTime, 0, _targetDistance);

            const float eps = 0.01f;

            if (_targetDistance <= eps ||
                Math.Abs(_treveledDistance - _targetDistance) <= eps)
            {
                _lastPoint = arrivalPoint;
                _track.Next();
                _targetDistance = 0;
                _treveledDistance = 0;

                return _lastPoint;
            }

            float t = _treveledDistance / _targetDistance;
            return Vector3.Lerp(_lastPoint, arrivalPoint, t);
        }

        public void AddPoint(Vector3 point)
        {
            _track.AddPoint(new Vector3(point.x, point.y, point.z));
        }

        public bool IsNeedMove => !_track.IsEmpty;

        public Vector3 GetLastPoint()
        {
            return _lastPoint;
        }
    }
}
