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
    class GrenadeFactory : RandomThingFactory
    {
        Random _rand;
        internal GrenadeFactory(Random rnd)
            : base(rnd, LootName.grenade) { }

        public override Thing Create(LootName name)
        {
            Thing thing = base.Create(name);
            if (null != thing)
                thing.Properties.Add(LootProperties.Damage, 15);

            return thing;
        }
    }
}
