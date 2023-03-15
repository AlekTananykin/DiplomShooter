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
    internal class RandomThingFactory: IThingFactory
    {
        private Random _rand;
        private LootName _thingName;
        protected RandomThingFactory(Random rnd, LootName name)
        {
            _rand = rnd;
            _thingName = name;
        }

        public virtual Thing Create(LootName name)
        {
            if (_thingName != name)
                return null;

            Thing thing = new Thing()
            {
                Name = _thingName,
                Cost = _rand.Next(12),
                Target = LootName.none
            };

            return thing;
        }

    }
}
