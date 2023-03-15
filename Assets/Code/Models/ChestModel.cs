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
    sealed  internal class ChestModel : ModelBase
    {
        public IList<Thing> Items;

        public Vector3 Position;
    }
}
