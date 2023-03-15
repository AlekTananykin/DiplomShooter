using Assets.Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Person.States
{
    class ToRun : IPersonState
    {
        public float Speed => 15f;
        public bool IsAttackEnable => false;
    }
}
