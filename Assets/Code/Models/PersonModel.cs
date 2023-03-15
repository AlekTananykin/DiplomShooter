using Assets.Code.Auxiliary;
using Assets.Code.Interfaces;
using Assets.Code.SaveLoad;
using Assets.Code.Things;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Models
{
    internal class PersonModel: ModelBase
    {
        public Vector3 InitPosition;
        public float Speed;

        public int Health;
        public int MaxHealth;
        public int LoadCapacity;

        public int Skill;

        private readonly IDictionary<LootName, IList<Thing>> bagItems;

        public IDictionary<LootName, IList<Thing>> GetBagItems()
        {
            return bagItems;
        }

        public IList<Thing> AppliedItems;

        public float RotationSpeed;

        public PersonModel()
        {
            bagItems = new Dictionary<LootName, IList<Thing>>();
            AppliedItems = new List<Thing>();
        }
    }
}
