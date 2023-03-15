using Assets.Code.Exceptions;
using Assets.Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Auxiliary
{
    internal sealed class LoopTrack : ITrack
    {
        List<Vector3> _points = new List<Vector3>();
        int _counter = 0;

        public void AddPoint(Vector3 point)
        {
            _points.Add(new Vector3(point.x, point.y, point.z));
            if (2 == this.PointsCount)
                Next();
        }

        public bool GetNextPoint(ref Vector3 point)
        {
            if (0 == _points.Count)
                return false;

            point = _points[_counter];
            _counter = (_counter + 1) % _points.Count;

            return true;
        }

        public Vector3 GetPoint()
        {
            if (_counter > _points.Count)
                throw new GameException("GetPoint: Out of range. ");

            return _points[_counter];
        }

        public bool Next()
        {
            if (0 == _points.Count)
                return false;

            _counter = (_counter + 1) % _points.Count;
            return true;
        }

        public bool IsEmpty => 0 == _points.Count;

        public int PointsCount => _points.Count;
    }
}
