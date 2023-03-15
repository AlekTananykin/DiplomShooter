using Assets.Code.Auxiliary;
using Assets.Code.Interfaces;
using Assets.Code.Models.ThingsFactories;
using Assets.Code.Things;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Models
{
    class CutlassFactory : RandomThingFactory
    {
        Random _rand;
        public CutlassFactory(Random rnd)
        : base(rnd, LootName.cutlass) { }

        public override Thing Create(LootName name)
        {
            Thing thing = base.Create(name);
            if (null != thing)
                thing.Properties.Add(LootProperties.Damage, 4);

            return thing;
        }
    }
}
