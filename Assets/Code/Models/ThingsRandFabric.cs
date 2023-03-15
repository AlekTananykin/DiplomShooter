using Assets.Code.Auxiliary;
using Assets.Code.Exceptions;
using Assets.Code.Interfaces;
using Assets.Code.Models.ThingsFactories;
using Assets.Code.Things;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Models
{
    internal sealed class ThingsRandFabric: IThingsFabric
    {
        private System.Random _rand = new System.Random();
        private IList<IThingFactory> _thingsFactories;

        internal ThingsRandFabric()
        {
            _thingsFactories = new List<IThingFactory>();
            _thingsFactories.Add(new CutlassFactory(_rand));
            _thingsFactories.Add(new GrenadeFactory(_rand));
            _thingsFactories.Add(new GunFactory(_rand));
            _thingsFactories.Add(new ScopeFactory(_rand));
            _thingsFactories.Add(new JackbootsFactory(_rand));
            _thingsFactories.Add(new CarryingBeltFactory(_rand));
            _thingsFactories.Add(new StoneOfLifeFactory(_rand));
            _thingsFactories.Add(new EncyclopediaFactory(_rand));
        }

        public IList<Thing> CreateThings(int thingsCount)
        {
            IList<Thing> things = new List<Thing>();
            Array values = Enum.GetValues(typeof(LootName));

            int thingNumber = 0;
            while (thingNumber < thingsCount)
            {
                LootName thingName = (LootName)values.GetValue(
                    _rand.Next(values.Length - 1));

                if (LootName.none == thingName)
                    continue;

                Thing thing = CreateThing(thingName);

                things.Add(thing);

                ++thingNumber;
            }

            return things;
        }

        private Thing CreateRandomThing(LootName thingName)
        {
            Thing thing = new Thing() { Name = thingName };

            Array values = Enum.GetValues(typeof(LootProperties));
            int propertyCount = _rand.Next(3);
            int propertyNumber = 0;

            while (propertyNumber < propertyCount)
            {
                LootProperties propertyName = (LootProperties)values.GetValue(
                    _rand.Next(values.Length - 1));

                if (thing.Properties.ContainsKey(propertyName))
                    continue;

                propertyNumber++;
                thing.Properties.Add(propertyName, _rand.Next(15));
            }

            return thing;
        }

        private Thing CreateThing(LootName thingName)
        {
            for (int i = 0; i < _thingsFactories.Count; ++i)
            {
                Thing thing = _thingsFactories[i].Create(thingName);
                if (null != thing)
                    return thing;
            }

            throw new GameException(
                "ThingsRandFabric: Unknown loot type. ");
        }
    }
}
