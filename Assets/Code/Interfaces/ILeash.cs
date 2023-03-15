using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Interfaces
{
    interface ILeash
    {
        Vector3 Execute(float deltaTime, float speed);
        void AddPoint(Vector3 point);
        bool IsNeedMove { get; }

        Vector3 GetLastPoint();
    }
}
