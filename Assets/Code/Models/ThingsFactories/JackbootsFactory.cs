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
    class JackbootsFactory : RandomThingFactory
    {
        public JackbootsFactory(Random rnd)
        : base(rnd, LootName.jackboots) { }
    }
}
