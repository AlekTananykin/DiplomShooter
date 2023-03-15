using Assets.Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Person.States
{
    class ToWalk : IPersonState
    {
        public float Speed => 10f;
        public bool IsAttackEnable => true;
    }
}
