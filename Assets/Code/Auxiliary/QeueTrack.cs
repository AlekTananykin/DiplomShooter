using Assets.Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Auxiliary
{
    internal class QeueTrack : ITrack
    {
        Queue<Vector3> _points = new Queue<Vector3>();

        public void AddPoint(Vector3 point)
        {
            _points.Enqueue(new Vector3(point.x, point.y, point.z));
        }

        public Vector3 GetPoint()
        {
            return _points.Peek();
        }

        public bool Next()
        {
            if (0 == _points.Count)
                return false;

            _points.Dequeue();
            if (0 == _points.Count)
                return false;

            return true;
        }

        public bool IsEmpty => 0 == _points.Count;

        public int PointsCount => _points.Count;
    }
}
