using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Interfaces
{
    interface IPersonMover
    {
        bool IsNeedMove { get; }
        void AddPoint(Vector3 point);

        (Vector3 nextPoint, Vector3 direction)
            Execute(float deltaTime, float speed);
    }
}
