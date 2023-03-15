using Assets.Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Models
{
    internal sealed class CameraModel : ModelBase
    {
        public float Speed;
        public Vector3 InitPosition;

        public Vector3 Forward;

        public float Height;
    }
}
