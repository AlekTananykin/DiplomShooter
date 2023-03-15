using Assets.Code.Auxiliary;
using Assets.Code.Interfaces;
using Assets.Code.Things;
using System;

namespace Assets.Code.Models.ThingsFactories
{
    internal class EncyclopediaFactory: RandomThingFactory
    {
        internal EncyclopediaFactory(Random rnd) 
            : base(rnd, LootName.encyclopedia) { }
    }
}
