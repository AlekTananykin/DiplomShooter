using Assets.Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Person.States
{
    internal class ToCrawl : IPersonState
    {
        public float Speed =>  0.2f;
        public bool IsAttackEnable => true;
    }
}
