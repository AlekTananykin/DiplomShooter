using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Interfaces
{
    interface ITrack
    {
        void AddPoint(Vector3 point);
        
        bool Next();
        Vector3 GetPoint();

        bool IsEmpty { get; }
        int PointsCount { get; }

    }
}
