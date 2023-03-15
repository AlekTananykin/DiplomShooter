using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Auxiliary
{
    internal class LoopLeash : Leash<LoopTrack>
    {
        public LoopLeash(Vector3 currentPoint)
            : base(currentPoint)
        {
             _track.AddPoint(currentPoint);
        }
    }
}
