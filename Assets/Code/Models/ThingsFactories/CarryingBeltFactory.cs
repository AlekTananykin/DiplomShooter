using Assets.Code.Auxiliary;
using Assets.Code.Interfaces;
using Assets.Code.Things;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Models.ThingsFactories
{
    internal class CarryingBeltFactory : RandomThingFactory
    {
        public CarryingBeltFactory(Random rnd)
        : base(rnd, LootName.carryingBelt) { }
    }
}
